using MediatR;
using MediaManager.Application.Interfaces;

namespace MediaManager.Application.UseCases.Commands.Media.DeleteMedia;

public class DeleteMediaHandler : IRequestHandler<DeleteMediaRequest>
{
    private readonly IMediaItemRepository _repository;

    public DeleteMediaHandler(IMediaItemRepository repository) => _repository = repository;

    public async Task Handle(DeleteMediaRequest request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.Id);
        var item = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"MediaItem {request.Id} não encontrado.");

        await _repository.DeleteAsync(item, cancellationToken);
    }
}
