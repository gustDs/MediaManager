using MediatR;
using MediaManager.Application.Interfaces;

namespace MediaManager.Application.UseCases.Queries.Media.GetMediaById;

public class GetMediaByIdHandler : IRequestHandler<GetMediaByIdRequest, GetMediaByIdResponse>
{
    private readonly IMediaItemRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public GetMediaByIdHandler(IMediaItemRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<GetMediaByIdResponse> Handle(GetMediaByIdRequest request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.Id);
        var item = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"MediaItem {request.Id} não encontrado.");

        if (item.UserId != _currentUserService.UserId)
            throw new KeyNotFoundException($"MediaItem {request.Id} não encontrado.");

        var statusAtual = item.ConsumptionRecords
            .OrderByDescending(cr => cr.CriadoEm)
            .FirstOrDefault()?.Status.ToString();

        return new GetMediaByIdResponse(item.Id, item.Nome, item.Tipo.ToString(), item.CriadoEm, statusAtual);
    }
}
