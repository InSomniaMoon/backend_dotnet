using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Users;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Users.Queries;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController(
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        IRequestHandler<GetUsersQuery, IEnumerable<UserResponse>> getAll,
        CancellationToken cancellationToken)
    {
        var result = await getAll.Handle(new GetUsersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}/structures")]
    public async Task<IActionResult> GetUserStructures(
        [FromRoute] int id,
        IRequestHandler<GetUserStructuresQuery, UserWithStructuresResponse?> getUserStructures,
        CancellationToken cancellationToken)
    {
        var result = await getUserStructures.Handle(new GetUserStructuresQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}

