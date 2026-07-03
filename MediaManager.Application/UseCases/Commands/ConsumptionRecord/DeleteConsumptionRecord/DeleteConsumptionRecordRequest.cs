using MediatR;

namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.DeleteConsumptionRecord;

public record DeleteConsumptionRecordRequest(string Id) : IRequest;
