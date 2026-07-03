using MediatR;

namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.CreateConsumptionRecord;

public record CreateConsumptionRecordRequest(
    string MediaItemId,
    string Status,
    DateTime? DataInicio,
    decimal? Nota,
    string? Resenha,
    int? HorasJogadas,
    int? PaginasLidas
) : IRequest<CreateConsumptionRecordResponse>;
