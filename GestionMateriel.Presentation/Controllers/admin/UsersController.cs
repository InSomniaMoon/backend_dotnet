using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Users;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Users.Queries;
using GestionMateriel.Application.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Admin;

[ApiController]
[Authorize("Admin")]
[Route("api/admin/users")]
public class UsersController(
) : ControllerBase
{
    [HttpGet("exists")]
    public async Task<IActionResult> UserExists(
        [FromQuery] string email,
        IRequestHandler<GetUserByEmailQuery, bool> getUserByEmail,
        CancellationToken cancellationToken)
    {
        return Ok(await getUserByEmail.Handle(new GetUserByEmailQuery(email), cancellationToken));
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request,
        IRequestHandler<CreateUserCommand, UserResponse> createUser,
        CancellationToken cancellationToken)
    {
        await createUser.Handle(new CreateUserCommand(request), cancellationToken);
        return Created();
    }
}
