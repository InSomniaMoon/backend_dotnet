using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.ItemIssues.Queries;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.admin;

[ApiController]
[Route("api/admin/issues")]
public class ItemIssueController(
    IRequestHandler<GetOpenItemIssuesQuery, IEnumerable<ItemIssueResponse>> getOpen
) : ControllerBase
{
    [HttpGet("open")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenIssues(CancellationToken cancellationToken)
    {
        var result = await getOpen.Handle(new GetOpenItemIssuesQuery(), cancellationToken);
        return Ok(result);
    }
}
