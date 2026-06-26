using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Backoffice.Commands;
using GestionMateriel.Application.Features.Backoffice.Queries;
using GestionMateriel.Application.Features.Users.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers.Backoffice;

// Route::prefix('/backoffice')->middleware('jwt:admin:app')->group(function () {
//   Route::get('/users', [UserController::class, 'getBackofficePaginatedUsers']);
//   Route::post('/users', [UserController::class, 'createUser']);
//   Route::get('/users/{user:id}/structures', [UserController::class, 'getUserStructures']);
//   Route::put('/users/{user:id}/structures', [UserController::class, 'updateUserStructures']);

//   Route::get('/structures', [StructureController::class, 'getStructures']);
//   Route::put('/structures/{structure:id}', [StructureController::class, 'update']);
//   Route::post('/structures/image', [StructureController::class, 'uploadFile']);
// });

[ApiController]
[Route("api/backoffice")]
[Authorize("AppAdmin")]

public class BackofficeController : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetPaginatedUsers(
    [FromQuery] PaginatedRequest request,
    IRequestHandler<PaginatedRequest, PaginatedResponse<UserResponse>> handler,
    CancellationToken cancellationToken
)
    {
        return Ok(await handler.Handle(request, cancellationToken));
    }

    [HttpPost("users")]
    public IActionResult CreateUser()
    {
        // Logic to create a user
        return Ok(new { message = "Create user" });
    }

    [HttpGet("users/{userId}/structures")]
    public async Task<IActionResult> GetUserStructures([FromRoute] int userId,
        IRequestHandler<GetUserStructuresQuery, List<StructureWithRoleResponse>> handler,
        CancellationToken cancellationToken)
    {

        var result = await handler.Handle(new GetUserStructuresQuery(userId), cancellationToken);
        // Logic to retrieve user structures
        return Ok(result);
    }

    [HttpPut("users/{userId}/structures")]
    public async Task<IActionResult> UpdateUserStructures([FromRoute] int userId,
    [FromBody] UpdateUserStructuresRequest request,
    IRequestHandler<UpdateUserStructuresCommand, bool> handler,
    CancellationToken cancellationToken)
    {

        var result = await handler.Handle(new UpdateUserStructuresCommand(userId, request.Structures), cancellationToken);
        // Logic to update user structures
        return Ok(result);
    }

    [HttpGet("structures")]
    public async Task<IActionResult> GetStructures(
        [FromQuery] PaginatedRequest request,
        IRequestHandler<PaginatedRequest, PaginatedResponse<StructureResponse>> handler,
        CancellationToken cancellationToken
    )
    {
        return Ok(await handler.Handle(request, cancellationToken));
    }

    [HttpPut("structures/{structureId}")]
    public IActionResult UpdateStructure(int structureId)
    {
        // Logic to update a structure
        return Ok(new { message = $"Update structure {structureId}" });
    }

    [HttpPost("structures/image")]
    public IActionResult UploadStructureImage()
    {
        // Logic to upload an image for a structure
        return Ok(new { message = "Upload structure image" });
    }
}