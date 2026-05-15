using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/events")]
public class EventsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request, CancellationToken cancellationToken)
    {
        if (request.ActualOnly)
        {
            var actual = await mediator.Send(new GetActualEventsQuery(), cancellationToken);
            return Ok(actual);
        }

        if (request.StructureId is null)
        {
            return BadRequest(new { message = "structureId is required when actualOnly is false." });
        }

        var events = await mediator.Send(new GetEventsByStructureQuery(request.StructureId.Value), cancellationToken);
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEventByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateEventCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetEventById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEvent([FromRoute] int id, [FromBody] UpdateEventRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateEventCommand(id, request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEvent([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteEventCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}/subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptions([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEventSubscriptionsQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/subscriptions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSubscription([FromRoute] int id, [FromBody] AddEventSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AddEventSubscriptionCommand(id, request), cancellationToken);
        return result is null ? NotFound() : StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{id:int}/subscriptions/{itemId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveSubscription([FromRoute] int id, [FromRoute] int itemId, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new RemoveEventSubscriptionCommand(id, itemId), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
