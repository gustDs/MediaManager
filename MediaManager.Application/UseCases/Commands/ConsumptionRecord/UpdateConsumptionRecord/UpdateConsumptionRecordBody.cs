namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.UpdateConsumptionRecord;

public record UpdateConsumptionRecordBody(
    string Status,
    DateTime? DataInicio,
    DateTime? DataFim,
    decimal? Nota,
    string? Resenha,
    int? HorasJogadas,
    int? PaginasLidas
);
