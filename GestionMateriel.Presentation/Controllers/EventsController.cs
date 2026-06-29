using System.Security.Claims;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/events")]
public class EventsController(
    IRequestHandler<GetEventsByStructureQuery, IEnumerable<EventResponse>> getByStructure,
    IRequestHandler<GetActualEventsQuery, IEnumerable<EventResponse>> getActual,
    IRequestHandler<GetEventByIdQuery, EventResponse?> getById,
    IRequestHandler<CreateEventCommand, EventResponse> create,
    IRequestHandler<UpdateEventCommand, EventResponse?> update,
    IRequestHandler<DeleteEventCommand, bool> delete,
    IRequestHandler<GetEventSubscriptionsQuery, IEnumerable<EventSubscriptionResponse>> getSubscriptions,
    IRequestHandler<AddEventSubscriptionCommand, EventSubscriptionResponse?> addSubscription,
    IRequestHandler<RemoveEventSubscriptionCommand, bool> removeSubscription
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request,
        CancellationToken cancellationToken)
    {

        var StructureType = User.Claims.FirstOrDefault(c => c.Type == "structureType")!.Value;
        var StructureCode = User.Claims.FirstOrDefault(c => c.Type == "structureCode")!.Value;

        var events = await getByStructure.Handle(new GetEventsByStructureQuery(request.StartDate, request.EndDate, StructureTypeEnum.FromString(StructureType), StructureCode), cancellationToken);
        return Ok(events);
    }

    [HttpGet("actual")]
    public async Task<IActionResult> GetActualEvents(CancellationToken cancellationToken)
    {
        var actual = await getActual.Handle(new GetActualEventsQuery(), cancellationToken);
        return Ok(actual);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await getById.Handle(new GetEventByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request,
        CancellationToken cancellationToken)
    {

        var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var result = await create.Handle(new CreateEventCommand(request, int.TryParse(UserId, out var userId) ? userId : 0), cancellationToken);
        return CreatedAtAction(nameof(GetEventById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEvent([FromRoute] int id, [FromBody] UpdateEventRequest request,
        CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdateEventCommand(id, request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEvent([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await delete.Handle(new DeleteEventCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}/subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptions([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await getSubscriptions.Handle(new GetEventSubscriptionsQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/subscriptions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSubscription([FromRoute] int id, [FromBody] AddEventSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await addSubscription.Handle(new AddEventSubscriptionCommand(id, request), cancellationToken);
        return result is null ? NotFound() : StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{id:int}/subscriptions/{itemId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveSubscription([FromRoute] int id, [FromRoute] int itemId,
        CancellationToken cancellationToken)
    {
        var deleted =
            await removeSubscription.Handle(new RemoveEventSubscriptionCommand(id, itemId), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
