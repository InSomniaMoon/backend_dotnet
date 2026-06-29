using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items.Issues;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.ItemIssues.Queries;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;

[ApiController]
[Authorize("Admin")]
[Route("api/admin/issues")]
public class ItemIssueController(
    IRequestHandler<GetOpenItemIssuesQuery, IEnumerable<ItemIssueResponse>> getOpen,
    IRequestHandler<GetItemIssueCommentsQuery, IEnumerable<ItemIssueCommentResponse>> getComments,
    IRequestHandler<ResolveItemIssueCommand, ItemIssueResponse?> resolve,
    IRequestHandler<CreateItemIssueCommentCommand, ItemIssueCommentResponse?> createComment

) : ControllerBase
{
    [HttpGet("open")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenIssues(CancellationToken cancellationToken)
    {
        var result = await getOpen.Handle(new GetOpenItemIssuesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:int}/resolve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResolveIssue([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await resolve.Handle(new ResolveItemIssueCommand(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:int}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComments([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await getComments.Handle(new GetItemIssueCommentsQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/comments")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateComment([FromRoute] int id, [FromBody] CreateItemIssueCommentRequest request,
    CancellationToken cancellationToken)
    {
        var result = await createComment.Handle(new CreateItemIssueCommentCommand(id, request), cancellationToken);
        return result is null ? NotFound() : StatusCode(StatusCodes.Status201Created, result);
    }

}
