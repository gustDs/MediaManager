using MediatR;
using MediaManager.Application.Interfaces;

namespace MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetAllConsumptionRecordsByMediaItem;

public class GetAllConsumptionRecordsByMediaItemHandler : IRequestHandler<GetAllConsumptionRecordsByMediaItemRequest, GetAllConsumptionRecordsByMediaItemResponse>
{
    private readonly IConsumptionRecordRepository _repository;

    public GetAllConsumptionRecordsByMediaItemHandler(IConsumptionRecordRepository repository) => _repository = repository;

    public async Task<GetAllConsumptionRecordsByMediaItemResponse> Handle(GetAllConsumptionRecordsByMediaItemRequest request, CancellationToken cancellationToken)
    {
        var mediaItemId = Guid.Parse(request.MediaItemId);
        var records = await _repository.GetAllByMediaItemIdAsync(mediaItemId, cancellationToken);

        var results = records.Select(r => new ConsumptionRecordResult(
            r.Id, r.MediaItemId, r.Status.ToString(),
            r.DataInicio, r.DataFim, r.Nota,
            r.Resenha, r.HorasJogadas, r.PaginasLidas, r.CriadoEm
        )).ToList();

        return new GetAllConsumptionRecordsByMediaItemResponse(results);
    }
}
