namespace GestionMateriel.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
}
