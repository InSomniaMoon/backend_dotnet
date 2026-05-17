using System.ComponentModel.DataAnnotations.Schema;

namespace GestionMateriel.Domain.Entities;

[Table("refresh_tokens")]
public class RefreshToken : BaseEntity
{
    [Column("token")]
    public string Token { get; set; } = string.Empty;
    [Column("user_id")]
    public int UserId { get; set; }
    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
}
