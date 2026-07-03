using MediatR;
using MediaManager.Application.Interfaces;
using MediaManager.Domain.Entities;
using MediaManager.Domain.Enums;

namespace MediaManager.Application.UseCases.Commands.Media.CreateMedia;

public class CreateMediaHandler : IRequestHandler<CreateMediaRequest, CreateMediaResponse>
{
    private readonly IMediaItemRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public CreateMediaHandler(IMediaItemRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<CreateMediaResponse> Handle(CreateMediaRequest request, CancellationToken cancellationToken)
    {
        var tipo = Enum.Parse<MediaType>(request.Tipo, ignoreCase: true);
        var item = new MediaItem(request.Nome, tipo, _currentUserService.UserId);

        await _repository.AddAsync(item, cancellationToken);

        return new CreateMediaResponse(item.Id, item.Nome, item.Tipo.ToString(), item.CriadoEm);
    }
}
