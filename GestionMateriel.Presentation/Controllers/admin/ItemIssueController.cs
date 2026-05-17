using GestionMateriel.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;

[ApiController]
[Route("api/admin/issues")]
public class ItemIssueController(IMediator mediator) : ControllerBase
{
    [HttpGet("open")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenIssues(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOpenItemIssuesQuery(), cancellationToken);
        return Ok(result);
    }
}
