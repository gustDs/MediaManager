using MediatR;

namespace MediaManager.Application.UseCases.Queries.Media.GetMediaById;

public record GetMediaByIdRequest(string Id) : IRequest<GetMediaByIdResponse>;
