namespace MediaManager.Application.UseCases.Commands.Media.UpdateMedia;

public record UpdateMediaResponse(Guid Id, string Nome, string Tipo, DateTime CriadoEm, string? StatusAtual);
