using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.admin;


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