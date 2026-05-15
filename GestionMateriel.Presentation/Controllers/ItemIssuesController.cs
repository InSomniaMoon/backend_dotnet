using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/item-issues")]
public class ItemIssuesController(IMediator mediator) : ControllerBase
{
    [HttpGet("open")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenIssues(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOpenItemIssuesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("by-item/{itemId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIssuesByItem([FromRoute] int itemId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetItemIssuesByItemQuery(itemId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateIssue([FromBody] CreateItemIssueRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateItemIssueCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetIssuesByItem), new { itemId = result.ItemId }, result);
    }

    [HttpPatch("{id:int}/resolve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResolveIssue([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ResolveItemIssueCommand(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:int}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComments([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetItemIssueCommentsQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/comments")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateComment([FromRoute] int id, [FromBody] CreateItemIssueCommentRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateItemIssueCommentCommand(id, request), cancellationToken);
        return result is null ? NotFound() : StatusCode(StatusCodes.Status201Created, result);
    }
}
