namespace GestionMateriel.Domain.Entities;

public class FeatureClick : BaseEntity
{
    public int FeatureId { get; set; }
    public int UserId { get; set; }
    
    // Navigation properties
    public Feature Feature { get; set; } = null!;
    public User User { get; set; } = null!;
}
