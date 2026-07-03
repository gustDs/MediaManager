using MediaManager.Application.Interfaces;
using MediaManager.Domain.Entities;
using MediaManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaManager.Infrastructure.Repositories;

public class ConsumptionRecordRepository : IConsumptionRecordRepository
{
    private readonly AppDbContext _context;

    public ConsumptionRecordRepository(AppDbContext context) => _context = context;

    public async Task<ConsumptionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.ConsumptionRecords
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<List<ConsumptionRecord>> GetAllByMediaItemIdAsync(Guid mediaItemId, CancellationToken cancellationToken = default)
        => await _context.ConsumptionRecords
            .Where(r => r.MediaItemId == mediaItemId)
            .OrderByDescending(r => r.CriadoEm)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(ConsumptionRecord record, CancellationToken cancellationToken = default)
    {
        _context.ConsumptionRecords.Add(record);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ConsumptionRecord record, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ConsumptionRecord record, CancellationToken cancellationToken = default)
    {
        _context.ConsumptionRecords.Remove(record);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
