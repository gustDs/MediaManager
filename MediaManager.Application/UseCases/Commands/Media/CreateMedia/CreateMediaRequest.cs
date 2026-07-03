using MediatR;

namespace MediaManager.Application.UseCases.Commands.Media.CreateMedia;

public record CreateMediaRequest(string Nome, string Tipo) : IRequest<CreateMediaResponse>;
