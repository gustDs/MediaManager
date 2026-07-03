using MediatR;
using MediaManager.Application.UseCases.Commands.ConsumptionRecord.CreateConsumptionRecord;
using MediaManager.Application.UseCases.Commands.ConsumptionRecord.UpdateConsumptionRecord;
using MediaManager.Application.UseCases.Commands.ConsumptionRecord.DeleteConsumptionRecord;
using MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetConsumptionRecordById;
using MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetAllConsumptionRecordsByMediaItem;
using Microsoft.AspNetCore.Mvc;

namespace MediaManager.API.Controllers;

[ApiController]
[Route("api/consumption-records")]
public class ConsumptionRecordController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConsumptionRecordController(IMediator mediator) => _mediator = mediator;

    [HttpGet("/api/media-items/{mediaItemId}/consumption-records")]
    public async Task<IActionResult> GetAllByMediaItem(string mediaItemId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllConsumptionRecordsByMediaItemRequest(mediaItemId), cancellationToken);
        return Ok(result.Records);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetConsumptionRecordByIdRequest(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConsumptionRecordRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateConsumptionRecordBody body, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new UpdateConsumptionRecordRequest(id, body.Status, body.DataInicio, body.DataFim,
                                               body.Nota, body.Resenha, body.HorasJogadas, body.PaginasLidas),
            cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteConsumptionRecordRequest(id), cancellationToken);
        return NoContent();
    }
}
