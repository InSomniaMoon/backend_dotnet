using System.Security.Claims;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Requests.Items.Issues;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.ItemIssues.Queries;
using GestionMateriel.Application.Features.Items.Commands;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Application.Features.Items.Requests;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ItemsController(
    IRequestHandler<GetItemsQuery, PaginatedResponse<ItemResponse>> getAll,
    IRequestHandler<GetItemByIdQuery, ItemResponse?> getById,
    IRequestHandler<CreateItemCommand, ItemResponse> create,
    IRequestHandler<CreateItemIssueCommand, ItemIssueResponse?> createItemIssue,
    IRequestHandler<UpdateItemCommand, ItemResponse?> update,
    IRequestHandler<DeleteItemCommand, bool> delete,
    IRequestHandler<GetAvailableItemsForDatesQuery, PaginatedResponse<ItemWithRestResponse>> getAvailableItemsForDates
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromQuery] GetPaginatedItemsRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetItemsQuery(request.Page, request.Size, request.Q, request.OrderDir, request.OrderBy, request.CategoryId);
        var result = await getAll.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("available")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableItemsForDates([FromQuery] GetAvailableItemsForDatesRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetAvailableItemsForDatesQuery(request.Page, request.Size, request.OrderBy, request.OrderDir, request.StartDate, request.EndDate, request.Q, request.ForEventId, request.CategoryId);
        query.Validate();
        var result = await getAvailableItemsForDates.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItemById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await getById.Handle(new GetItemByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await create.Handle(new CreateItemCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetItemById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] UpdateItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdateItemCommand(id, request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await delete.Handle(new DeleteItemCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/issues")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateItemIssue([FromRoute] int id, [FromBody] CreateItemIssueRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result =
            await createItemIssue.Handle(new CreateItemIssueCommand(id, int.Parse(userId!), request),
                cancellationToken);
        return result is null
            ? NotFound()
            : CreatedAtAction(nameof(GetItemIssues), new { id }, result);
    }

    [HttpGet("{id:int}/issues")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetItemIssues([FromRoute] int id,
        IRequestHandler<GetItemIssuesByItemQuery, IEnumerable<ItemIssueResponse>> getItemIssues,
        CancellationToken cancellationToken)
    {
        var result = await getItemIssues.Handle(new GetItemIssuesByItemQuery(id), cancellationToken);
        return Ok(result);
    }
}
