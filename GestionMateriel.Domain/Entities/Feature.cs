namespace GestionMateriel.Domain.Entities;

public class Feature : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<FeatureClick> Clicks { get; set; } = [];
}
