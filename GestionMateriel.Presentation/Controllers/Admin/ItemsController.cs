using System.Text.Json;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.ItemIssues.Queries;
using GestionMateriel.Application.Features.Items.Commands;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;

[ApiController]
[Authorize("Admin")]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[Route("api/admin/items")]
public class ItemsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request,
        IRequestHandler<CreateItemCommand, ItemResponse> create, CancellationToken cancellationToken)
    {
        var result = await create.Handle(new CreateItemCommand(request), cancellationToken);
        return Created("", result);
    }

    [HttpPost("images")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadItemImage([FromForm] IFormFile image,
        IRequestHandler<UploadImageCommand, ImageCreatedResponse> uploadImage,
        CancellationToken cancellationToken)
    {
        if (image is null || image.Length == 0)
            return BadRequest("No file uploaded.");

        await using var stream = image.OpenReadStream();
        var result =
            await uploadImage.Handle(new UploadImageCommand(stream, "items", image.FileName), cancellationToken);
        return Created("", result);
    }

    [HttpPost("import/preview")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PreviewImportItems([FromForm] IFormFile file,
        IRequestHandler<PreviewImportItemsCommand, PreviewImportItemsResponse> previewImportItems,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file uploaded.");

        await using var stream = file.OpenReadStream();

        var structureId = User.Claims.FirstOrDefault(c => c.Type == "structureId")!.Value!;

        var result =
            await previewImportItems.Handle(new PreviewImportItemsCommand(stream, file.FileName, int.Parse(structureId), cancellationToken), cancellationToken);
        return Ok(result);
    }


    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] UpdateItemRequest request,
        IRequestHandler<UpdateItemCommand, ItemResponse?> update, CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdateItemCommand(id, request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:int}/issues")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetItemIssues([FromRoute] int id,
        IRequestHandler<GetItemIssuesByItemQuery, IEnumerable<ItemIssueResponse>> getItemIssues,
        CancellationToken cancellationToken)
    {
        var result = await getItemIssues.Handle(new GetItemIssuesByItemQuery(id), cancellationToken);
        return Ok(result);
    }
}