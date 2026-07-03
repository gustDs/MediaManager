using MediatR;
using MediaManager.Application.Interfaces;

namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.DeleteConsumptionRecord;

public class DeleteConsumptionRecordHandler : IRequestHandler<DeleteConsumptionRecordRequest>
{
    private readonly IConsumptionRecordRepository _repository;

    public DeleteConsumptionRecordHandler(IConsumptionRecordRepository repository) => _repository = repository;

    public async Task Handle(DeleteConsumptionRecordRequest request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.Id);
        var record = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"ConsumptionRecord {request.Id} não encontrado.");

        await _repository.DeleteAsync(record, cancellationToken);
    }
}
