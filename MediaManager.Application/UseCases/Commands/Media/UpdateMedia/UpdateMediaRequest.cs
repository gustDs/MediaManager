using MediatR;

namespace MediaManager.Application.UseCases.Commands.Media.UpdateMedia;

public record UpdateMediaRequest(string Id, string Nome) : IRequest<UpdateMediaResponse>;
