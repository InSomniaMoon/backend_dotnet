namespace GestionMateriel.Domain.Entities;

public class ItemCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int StructureId { get; set; }
    public bool Identified { get; set; }

    // Navigation properties
    public Structure Structure { get; set; } = null!;
    public ICollection<Item> Items { get; set; } = [];
}
