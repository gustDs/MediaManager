namespace MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetConsumptionRecordById;

public record GetConsumptionRecordByIdResponse(
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
