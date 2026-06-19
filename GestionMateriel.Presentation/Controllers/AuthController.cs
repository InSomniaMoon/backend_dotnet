using System.Security.Claims;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionMateriel.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);
        if (response is null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await authService.RegisterAsync(request, cancellationToken);
            return CreatedAtAction(nameof(Register), response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.RefreshTokenAsync(request, cancellationToken);
        if (response is null)
        {
            return Unauthorized(new { message = "Refresh token is invalid or expired." });
        }

        return Ok(response);
    }

    [HttpPost("select-structure/{structureId:int}")]
    public async Task<IActionResult> SelectStructure(int structureId, [FromBody] SelectStructureRequest request, CancellationToken cancellationToken)
    {
        var userId = User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var response = await authService.SelectStructureAsync(int.Parse(userId!), structureId, request, cancellationToken);
        if (response is null)
        {
            return Unauthorized(new { message = "Invalid structure selection." });
        }

        return Ok(response);
    }
}
