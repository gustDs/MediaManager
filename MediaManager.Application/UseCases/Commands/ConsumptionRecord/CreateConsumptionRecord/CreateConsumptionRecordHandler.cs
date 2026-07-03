using MediatR;
using MediaManager.Application.Interfaces;
using MediaManager.Domain.Enums;

namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.CreateConsumptionRecord;

public class CreateConsumptionRecordHandler : IRequestHandler<CreateConsumptionRecordRequest, CreateConsumptionRecordResponse>
{
    private readonly IConsumptionRecordRepository _repository;
    private readonly IMediaItemRepository _mediaItemRepository;

    public CreateConsumptionRecordHandler(
        IConsumptionRecordRepository repository,
        IMediaItemRepository mediaItemRepository)
    {
        _repository = repository;
        _mediaItemRepository = mediaItemRepository;
    }

    public async Task<CreateConsumptionRecordResponse> Handle(CreateConsumptionRecordRequest request, CancellationToken cancellationToken)
    {
        var mediaItemId = Guid.Parse(request.MediaItemId);
        var mediaItem = await _mediaItemRepository.GetByIdAsync(mediaItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"MediaItem {request.MediaItemId} não encontrado.");

        if (request.HorasJogadas.HasValue && mediaItem.Tipo != MediaType.Jogo)
            throw new ArgumentException("HorasJogadas só é permitido para itens do tipo Jogo.");

        if (request.PaginasLidas.HasValue && mediaItem.Tipo != MediaType.Livro)
            throw new ArgumentException("PaginasLidas só é permitido para itens do tipo Livro.");

        var status = Enum.Parse<ConsumptionStatus>(request.Status, ignoreCase: true);

        var record = new Domain.Entities.ConsumptionRecord(
            mediaItemId,
            status,
            request.DataInicio,
            request.Nota,
            request.Resenha,
            request.HorasJogadas,
            request.PaginasLidas
        );

        await _repository.AddAsync(record, cancellationToken);

        return new CreateConsumptionRecordResponse(
            record.Id, record.MediaItemId, record.Status.ToString(),
            record.DataInicio, record.DataFim, record.Nota,
            record.Resenha, record.HorasJogadas, record.PaginasLidas, record.CriadoEm
        );
    }
}
