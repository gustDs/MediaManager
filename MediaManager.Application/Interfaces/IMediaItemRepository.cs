using MediaManager.Domain.Entities;

namespace MediaManager.Application.Interfaces;

public interface IMediaItemRepository
{
    Task<MediaItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<MediaItem>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(MediaItem item, CancellationToken cancellationToken = default);
    Task UpdateAsync(MediaItem item, CancellationToken cancellationToken = default);
    Task DeleteAsync(MediaItem item, CancellationToken cancellationToken = default);
}
