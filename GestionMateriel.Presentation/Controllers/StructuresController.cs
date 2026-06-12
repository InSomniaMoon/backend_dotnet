using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/structures")]
public class StructuresController(
    IRequestHandler<GetStructuresQuery, IEnumerable<StructureResponse>> getAll,
    IRequestHandler<GetStructureByIdQuery, StructureResponse?> getById,
    IRequestHandler<GetStructureMembersQuery, IEnumerable<StructureMemberResponse>> getMembers,
    IRequestHandler<CreateStructureCommand, StructureResponse> create,
    IRequestHandler<UpdateStructureCommand, StructureResponse?> update,
    IRequestHandler<AddUserToStructureCommand, StructureMemberResponse?> addMember
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStructures(CancellationToken cancellationToken)
    {
        var result = await getAll.Handle(new GetStructuresQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStructureById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await getById.Handle(new GetStructureByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:int}/members")]
    public async Task<IActionResult> GetMembers([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await getMembers.Handle(new GetStructureMembersQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStructure([FromBody] CreateStructureRequest request, CancellationToken cancellationToken)
    {
        var result = await create.Handle(new CreateStructureCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetStructureById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStructure([FromRoute] int id, [FromBody] UpdateStructureRequest request, CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdateStructureCommand(id, request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("members")]
    public async Task<IActionResult> AddUserToStructure([FromBody] AddUserToStructureRequest request, CancellationToken cancellationToken)
    {
        var result = await addMember.Handle(new AddUserToStructureCommand(request), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
