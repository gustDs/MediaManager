using MediatR;
using MediaManager.Application.Interfaces;

namespace MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetConsumptionRecordById;

public class GetConsumptionRecordByIdHandler : IRequestHandler<GetConsumptionRecordByIdRequest, GetConsumptionRecordByIdResponse>
{
    private readonly IConsumptionRecordRepository _repository;

    public GetConsumptionRecordByIdHandler(IConsumptionRecordRepository repository) => _repository = repository;

    public async Task<GetConsumptionRecordByIdResponse> Handle(GetConsumptionRecordByIdRequest request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.Id);
        var record = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"ConsumptionRecord {request.Id} não encontrado.");

        return new GetConsumptionRecordByIdResponse(
            record.Id, record.MediaItemId, record.Status.ToString(),
            record.DataInicio, record.DataFim, record.Nota,
            record.Resenha, record.HorasJogadas, record.PaginasLidas, record.CriadoEm
        );
    }
}
