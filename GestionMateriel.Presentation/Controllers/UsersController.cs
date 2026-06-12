using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Users;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController(
    IRequestHandler<GetUsersQuery, IEnumerable<UserResponse>> getAll,
    IRequestHandler<GetUserStructuresQuery, UserWithStructuresResponse?> getUserStructures,
    IRequestHandler<CreateUserCommand, UserResponse> create
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var result = await getAll.Handle(new GetUsersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}/structures")]
    public async Task<IActionResult> GetUserStructures([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await getUserStructures.Handle(new GetUserStructuresQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await create.Handle(new CreateUserCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetUsers), new { id = result.Id }, result);
    }
}
