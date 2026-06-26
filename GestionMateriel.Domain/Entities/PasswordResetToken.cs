using System.ComponentModel.DataAnnotations.Schema;

namespace GestionMateriel.Domain.Entities;

[Table("password_reset_tokens")]
public class PasswordResetToken
{
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("token")]
    public string Token { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
