namespace MediaManager.Application.UseCases.Queries.Media.GetAllMedia;

public record MediaItemResult(Guid Id, string Nome, string Tipo, DateTime CriadoEm, string? StatusAtual);

public record GetAllMediaResponse(IReadOnlyList<MediaItemResult> Items);
