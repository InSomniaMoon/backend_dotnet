namespace GestionMateriel.Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int StructureId { get; set; }
    public bool Usable { get; set; } = true;
    public int Stock { get; set; }
    public DateTime? DateOfBuy { get; set; }
    public string? ImagePath { get; set; }
    public string? CodeStructure { get; set; }

    // Navigation properties
    public ItemCategory Category { get; set; } = null!;
    public Structure Structure { get; set; } = null!;
    public ICollection<ItemIssue> Issues { get; set; } = [];
    public ICollection<EventSubscription> EventSubscriptions { get; set; } = [];
}
