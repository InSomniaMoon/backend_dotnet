using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Features.ItemIssues.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;

[ApiController]
[Route("api/admin/items")]
public class ItemsController : ControllerBase
{
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] UpdateItemRequest request,
        IRequestHandler<UpdateItemCommand, ItemResponse?> update, CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdateItemCommand(id, request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
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