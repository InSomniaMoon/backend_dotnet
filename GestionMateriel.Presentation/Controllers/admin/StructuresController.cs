using GestionMateriel.Application.DTOs.Responses.Structures;
using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class StructuresController(
    IRequestHandler<GetCurrentStructureWithChildrenQuery, StructureWithChildrenResponse> getCurrentStructureWithChildren
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<StructureWithChildrenResponse>> GetCurrentStructureWithChildren(
        CancellationToken cancellationToken)
    {
        // Implementation for retrieving structures
        return Ok(await getCurrentStructureWithChildren.Handle(new GetCurrentStructureWithChildrenQuery(1),
            cancellationToken));
    }
}