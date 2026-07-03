using MediatR;
using MediaManager.Application.UseCases.Commands.Media.CreateMedia;
using MediaManager.Application.UseCases.Commands.Media.UpdateMedia;
using MediaManager.Application.UseCases.Commands.Media.DeleteMedia;
using MediaManager.Application.UseCases.Queries.Media.GetMediaById;
using MediaManager.Application.UseCases.Queries.Media.GetAllMedia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/media-items")]
public class MediaItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediaItemController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllMediaRequest(), cancellationToken);
        return Ok(result.Items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMediaByIdRequest(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMediaRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateMediaBody body, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateMediaRequest(id, body.Nome), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteMediaRequest(id), cancellationToken);
        return NoContent();
    }
}
