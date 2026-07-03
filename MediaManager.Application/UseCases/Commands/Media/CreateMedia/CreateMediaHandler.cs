using MediatR;
using MediaManager.Application.Interfaces;
using MediaManager.Domain.Entities;
using MediaManager.Domain.Enums;

namespace MediaManager.Application.UseCases.Commands.Media.CreateMedia;

public class CreateMediaHandler : IRequestHandler<CreateMediaRequest, CreateMediaResponse>
{
    private readonly IMediaItemRepository _repository;

    public CreateMediaHandler(IMediaItemRepository repository) => _repository = repository;

    public async Task<CreateMediaResponse> Handle(CreateMediaRequest request, CancellationToken cancellationToken)
    {
        var tipo = Enum.Parse<MediaType>(request.Tipo, ignoreCase: true);
        var item = new MediaItem(request.Nome, tipo);

        await _repository.AddAsync(item, cancellationToken);

        return new CreateMediaResponse(item.Id, item.Nome, item.Tipo.ToString(), item.CriadoEm);
    }
}
