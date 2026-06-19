using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Categories;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Categories.Queries;
using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/items/categories")]
public class ItemCategoriesController(
    IRequestHandler<GetItemCategoriesQuery, IEnumerable<ItemCategoryResponse>> getAll,
    IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryResponse?> getById
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories([FromQuery] GetItemCategoriesRequest request,
        CancellationToken cancellationToken)
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
}
