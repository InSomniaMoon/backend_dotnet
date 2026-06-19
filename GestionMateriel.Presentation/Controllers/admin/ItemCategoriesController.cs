using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Categories;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Categories.Queries;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Admin.Controllers;

[ApiController]
[Authorize]
[Route("api/admin/items/categories")]
public class ItemCategoriesController(
    IRequestHandler<CreateItemCategoryCommand, ItemCategoryResponse> create,
    IRequestHandler<UpdateItemCategoryCommand, ItemCategoryResponse?> update,
    IRequestHandler<DeleteItemCategoryCommand, bool> delete
) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateItemCategoryRequest request,
     CancellationToken cancellationToken)
    {
        // get strcture id from user claims
        var structureIdClaim = User.Claims.FirstOrDefault(c => c.Type == "structureId");
        var codeStructureClaim = User.Claims.FirstOrDefault(c => c.Type == "structureCode");
        var result = await create.Handle(new CreateItemCategoryCommand(request, int.Parse(structureIdClaim!.Value), codeStructureClaim?.Value), cancellationToken);
        return Created("", result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateItemCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdateItemCategoryCommand(id, request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await delete.Handle(new DeleteItemCategoryCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
