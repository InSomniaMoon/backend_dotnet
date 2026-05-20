using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;


[ApiController]
[Route("api/[controller]")]
public class StructuresController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetStructures()
    {
        // Implementation for retrieving structures
        return Ok();
    }
}