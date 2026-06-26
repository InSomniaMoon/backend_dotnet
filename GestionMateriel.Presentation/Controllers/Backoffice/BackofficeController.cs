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
[Route("/api/backoffice")]
[Authorize(Policy = "AppAdmin")]

public class BackofficeController : ControllerBase
{
    [HttpGet("users")]
    public IActionResult GetPaginatedUsers()
    {
        // Logic to retrieve users
        return Ok(new { message = "Get users" });
    }

    [HttpPost("users")]
    public IActionResult CreateUser()
    {
        // Logic to create a user
        return Ok(new { message = "Create user" });
    }

    [HttpGet("users/{userId}/structures")]
    public IActionResult GetUserStructures(int userId)
    {
        // Logic to retrieve user structures
        return Ok(new { message = $"Get structures for user {userId}" });
    }

    [HttpPut("users/{userId}/structures")]
    public IActionResult UpdateUserStructures(int userId)
    {
        // Logic to update user structures
        return Ok(new { message = $"Update structures for user {userId}" });
    }

    [HttpGet("structures")]
    public IActionResult GetStructures()
    {
        // Logic to retrieve structures
        return Ok(new { message = "Get structures" });
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