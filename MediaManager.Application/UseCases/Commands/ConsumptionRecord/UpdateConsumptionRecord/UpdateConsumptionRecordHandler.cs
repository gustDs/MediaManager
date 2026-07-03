using MediatR;
using MediaManager.Application.Interfaces;
using MediaManager.Domain.Enums;

namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.UpdateConsumptionRecord;

public class UpdateConsumptionRecordHandler : IRequestHandler<UpdateConsumptionRecordRequest, UpdateConsumptionRecordResponse>
{
    private readonly IConsumptionRecordRepository _repository;
    private readonly IMediaItemRepository _mediaItemRepository;

    public UpdateConsumptionRecordHandler(
        IConsumptionRecordRepository repository,
        IMediaItemRepository mediaItemRepository)
    {
        _repository = repository;
        _mediaItemRepository = mediaItemRepository;
    }

    public async Task<UpdateConsumptionRecordResponse> Handle(UpdateConsumptionRecordRequest request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.Id);
        var record = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"ConsumptionRecord {request.Id} não encontrado.");

        var mediaItem = await _mediaItemRepository.GetByIdAsync(record.MediaItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"MediaItem {record.MediaItemId} não encontrado.");

        if (request.HorasJogadas.HasValue && mediaItem.Tipo != MediaType.Jogo)
            throw new ArgumentException("HorasJogadas só é permitido para itens do tipo Jogo.");

        if (request.PaginasLidas.HasValue && mediaItem.Tipo != MediaType.Livro)
            throw new ArgumentException("PaginasLidas só é permitido para itens do tipo Livro.");

        var status = Enum.Parse<ConsumptionStatus>(request.Status, ignoreCase: true);

        record.Update(status, request.DataInicio, request.DataFim, request.Nota,
                      request.Resenha, request.HorasJogadas, request.PaginasLidas);

        await _repository.UpdateAsync(record, cancellationToken);

        return new UpdateConsumptionRecordResponse(
            record.Id, record.MediaItemId, record.Status.ToString(),
            record.DataInicio, record.DataFim, record.Nota,
            record.Resenha, record.HorasJogadas, record.PaginasLidas, record.CriadoEm
        );
    }
}
