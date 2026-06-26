using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.DTOs.Responses.Structures;
using GestionMateriel.Application.Features.Structures.Commands;
using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class StructuresController(
    IRequestHandler<GetCurrentStructureWithChildrenQuery, StructureWithChildrenResponse>
        getCurrentStructureWithChildren,
    IRequestHandler<GetStructureMembersQuery, PaginatedResponse<UserWithStructuresResponse>> getStructureMembers
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<StructureWithChildrenResponse>> GetCurrentStructureWithChildren(
        CancellationToken cancellationToken)
    {
        var structureId = User.Claims.FirstOrDefault(c => c.Type == "structureId")?.Value!;

        // Implementation for retrieving structures
        return Ok(await getCurrentStructureWithChildren.Handle(new GetCurrentStructureWithChildrenQuery(int.Parse(structureId)),
            cancellationToken));
    }

    [HttpGet("{structureId:int}/users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<UserWithStructuresResponse>>> GetStructureUsers(int structureId,
        [FromQuery] PaginatedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await getStructureMembers.Handle(
            new GetStructureMembersQuery(structureId, request.Page, request.Size, request.Q, request.OrderDir,
                request.OrderBy),
            cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{structureId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStructure(int structureId, [FromBody] UpdateStructureRequest request,
        IRequestHandler<UpdateStructureCommand, StructureResponse?> updateStructure,
        CancellationToken cancellationToken)
    {
        var result = await updateStructure.Handle(new UpdateStructureCommand(structureId, request.Color!, request.Name, request.Members, request.Image), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("images")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadStructureImage([FromForm] IFormFile image,
       IRequestHandler<UploadImageCommand, ImageCreatedResponse> uploadImage,
       CancellationToken cancellationToken)
    {
        if (image is null || image.Length == 0)
            return BadRequest("No file uploaded.");

        await using var stream = image.OpenReadStream();
        var result =
            await uploadImage.Handle(new UploadImageCommand(stream, "structures", image.FileName), cancellationToken);
        return Created("", result);
    }

}