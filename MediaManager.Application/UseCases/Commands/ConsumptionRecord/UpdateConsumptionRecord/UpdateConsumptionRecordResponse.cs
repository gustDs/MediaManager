namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.UpdateConsumptionRecord;

public record UpdateConsumptionRecordResponse(
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
