using MediaManager.Application.Interfaces;
using MediaManager.Domain.Entities;
using MediaManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaManager.Infrastructure.Repositories;

public class MediaItemRepository : IMediaItemRepository
{
    private readonly AppDbContext _context;

    public MediaItemRepository(AppDbContext context) => _context = context;

    public async Task<MediaItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.MediaItems
            .Include(m => m.ConsumptionRecords)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<List<MediaItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.MediaItems
            .Include(m => m.ConsumptionRecords)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(MediaItem item, CancellationToken cancellationToken = default)
    {
        _context.MediaItems.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(MediaItem item, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(MediaItem item, CancellationToken cancellationToken = default)
    {
        _context.MediaItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
