using MediatR;

namespace MediaManager.Application.UseCases.Queries.Media.GetAllMedia;

public record GetAllMediaRequest : IRequest<GetAllMediaResponse>;
