using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;

[ApiController]
[Route("api/admin/items")]
public class ItemsController(IMediator mediator) : ControllerBase
{

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] UpdateItemRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateItemCommand(id, request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}