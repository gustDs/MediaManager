namespace MediaManager.Application.UseCases.Commands.Media.CreateMedia;

public record CreateMediaResponse(Guid Id, string Nome, string Tipo, DateTime CriadoEm);
