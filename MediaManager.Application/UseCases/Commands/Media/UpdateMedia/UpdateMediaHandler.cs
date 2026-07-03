using MediatR;
using MediaManager.Application.Interfaces;

namespace MediaManager.Application.UseCases.Commands.Media.UpdateMedia;

public class UpdateMediaHandler : IRequestHandler<UpdateMediaRequest, UpdateMediaResponse>
{
    private readonly IMediaItemRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateMediaHandler(IMediaItemRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateMediaResponse> Handle(UpdateMediaRequest request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.Id);
        var item = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"MediaItem {request.Id} não encontrado.");

        if (item.UserId != _currentUserService.UserId)
            throw new KeyNotFoundException($"MediaItem {request.Id} não encontrado.");

        item.Update(request.Nome);
        await _repository.UpdateAsync(item, cancellationToken);

        var statusAtual = item.ConsumptionRecords
            .OrderByDescending(cr => cr.CriadoEm)
            .FirstOrDefault()?.Status.ToString();

        return new UpdateMediaResponse(item.Id, item.Nome, item.Tipo.ToString(), item.CriadoEm, statusAtual);
    }
}
