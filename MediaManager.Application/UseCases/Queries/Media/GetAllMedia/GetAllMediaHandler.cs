using MediatR;
using MediaManager.Application.Interfaces;

namespace MediaManager.Application.UseCases.Queries.Media.GetAllMedia;

public class GetAllMediaHandler : IRequestHandler<GetAllMediaRequest, GetAllMediaResponse>
{
    private readonly IMediaItemRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllMediaHandler(IMediaItemRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<GetAllMediaResponse> Handle(GetAllMediaRequest request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(_currentUserService.UserId, cancellationToken);

        var results = items.Select(item =>
        {
            var statusAtual = item.ConsumptionRecords
                .OrderByDescending(cr => cr.CriadoEm)
                .FirstOrDefault()?.Status.ToString();

            return new MediaItemResult(item.Id, item.Nome, item.Tipo.ToString(), item.CriadoEm, statusAtual);
        }).ToList();

        return new GetAllMediaResponse(results);
    }
}
