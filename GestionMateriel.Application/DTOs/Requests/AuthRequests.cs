using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty;

    [Phone]
    [MaxLength(20)]
    public string? Phone { get; set; }
}

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public class SelectStructureRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;

}