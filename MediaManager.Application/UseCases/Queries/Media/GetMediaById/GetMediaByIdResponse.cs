namespace MediaManager.Application.UseCases.Queries.Media.GetMediaById;

public record GetMediaByIdResponse(Guid Id, string Nome, string Tipo, DateTime CriadoEm, string? StatusAtual);
