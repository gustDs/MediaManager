using MediaManager.Domain.Entities;

namespace MediaManager.Application.Interfaces;

public interface IConsumptionRecordRepository
{
    Task<ConsumptionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ConsumptionRecord>> GetAllByMediaItemIdAsync(Guid mediaItemId, CancellationToken cancellationToken = default);
    Task AddAsync(ConsumptionRecord record, CancellationToken cancellationToken = default);
    Task UpdateAsync(ConsumptionRecord record, CancellationToken cancellationToken = default);
    Task DeleteAsync(ConsumptionRecord record, CancellationToken cancellationToken = default);
}
