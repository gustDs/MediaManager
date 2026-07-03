using MediatR;

namespace MediaManager.Application.UseCases.Commands.Media.DeleteMedia;

public record DeleteMediaRequest(string Id) : IRequest;
