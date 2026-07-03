namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.CreateConsumptionRecord;

public record CreateConsumptionRecordResponse(
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
