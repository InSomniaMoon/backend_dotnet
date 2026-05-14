using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Domain.Entities;

public class Structure : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string CodeStructure { get; set; } = string.Empty;
    public string NomStructure { get; set; } = string.Empty;
    public StructureTypeEnum Type { get; set; }
    public string? ParentCode { get; set; }
    public string? Color { get; set; }
    public string? ImagePath { get; set; }

    // Navigation properties
    public ICollection<UserStructure> UserStructures { get; set; } = [];
    public ICollection<Item> Items { get; set; } = [];
    public ICollection<ItemCategory> ItemCategories { get; set; } = [];
    public ICollection<Event> Events { get; set; } = [];
}
