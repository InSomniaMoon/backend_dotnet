using System.ComponentModel.DataAnnotations.Schema;

namespace GestionMateriel.Domain.Entities;

[Table("features")]
public class Feature : BaseEntity
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("slug")]
    public string Slug { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<FeatureClick> Clicks { get; set; } = [];
}
