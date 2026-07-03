using MediatR;

namespace MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetConsumptionRecordById;

public record GetConsumptionRecordByIdRequest(string Id) : IRequest<GetConsumptionRecordByIdResponse>;
