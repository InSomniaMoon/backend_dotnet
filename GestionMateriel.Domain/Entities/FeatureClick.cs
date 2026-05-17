using System.ComponentModel.DataAnnotations.Schema;

namespace GestionMateriel.Domain.Entities;

[Table("feature_clicks")]
public class FeatureClick : BaseEntity
{
    [Column("feature_id")]
    public int FeatureId { get; set; }
    [Column("user_id")]
    public int UserId { get; set; }

    // Navigation properties
    public virtual Feature Feature { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
