namespace MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetAllConsumptionRecordsByMediaItem;

public record ConsumptionRecordResult(
    Guid Id,
    Guid MediaItemId,
    string Status,
    DateTime? DataInicio,
    DateTime? DataFim,
    decimal? Nota,
    string? Resenha,
    int? HorasJogadas,
    int? PaginasLidas,
    DateTime CriadoEm
);

public record GetAllConsumptionRecordsByMediaItemResponse(IReadOnlyList<ConsumptionRecordResult> Records);
