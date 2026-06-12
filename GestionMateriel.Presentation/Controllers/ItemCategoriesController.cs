using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Categories;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/items/categories")]
public class ItemCategoriesController(
    IRequestHandler<GetItemCategoriesQuery, IEnumerable<ItemCategoryResponse>> getAll,
    IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryResponse?> getById,
    IRequestHandler<CreateItemCategoryCommand, ItemCategoryResponse> create,
    IRequestHandler<UpdateItemCategoryCommand, ItemCategoryResponse?> update,
    IRequestHandler<DeleteItemCategoryCommand, bool> delete
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories([FromQuery] GetItemCategoriesRequest request, CancellationToken cancellationToken)
    {
        var result = await getAll.Handle(new GetItemCategoriesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await getById.Handle(new GetItemCategoryByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateItemCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await create.Handle(new CreateItemCategoryCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateItemCategoryRequest request, CancellationToken cancellationToken)
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
