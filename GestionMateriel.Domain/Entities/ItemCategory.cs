using System.ComponentModel.DataAnnotations.Schema;
using GestionMateriel.Domain.Interfaces;

namespace GestionMateriel.Domain.Entities;

[Table("item_categories")]
public class ItemCategory : BaseEntity, IStructureCodeScopedEntity
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("structure_id")]
    public int StructureId { get; set; }
    [Column("identified")]
    public bool Identified { get; set; }

    // Navigation properties
    public virtual Structure Structure { get; set; } = null!;
    public virtual ICollection<Item> Items { get; set; } = [];
    [Column("code_structure")]
    public string? CodeStructure { get; set; }
}
