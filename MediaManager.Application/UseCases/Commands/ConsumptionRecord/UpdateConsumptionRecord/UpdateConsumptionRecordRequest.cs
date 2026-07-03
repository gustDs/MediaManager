using MediatR;

namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.UpdateConsumptionRecord;

public record UpdateConsumptionRecordRequest(
    string Id,
    string Status,
    DateTime? DataInicio,
    DateTime? DataFim,
    decimal? Nota,
    string? Resenha,
    int? HorasJogadas,
    int? PaginasLidas
) : IRequest<UpdateConsumptionRecordResponse>;
