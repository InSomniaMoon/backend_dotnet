using System.ComponentModel.DataAnnotations.Schema;
using GestionMateriel.Domain.Interfaces;

namespace GestionMateriel.Domain.Entities;

[Table("items")]
public class Item : BaseEntity, IStructureCodeScopedEntity
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("description")]
    public string? Description { get; set; }
    [Column("category_id")]
    public int CategoryId { get; set; }
    [Column("structure_id")]
    public int StructureId { get; set; }
    [Column("usable")]
    public bool Usable { get; set; } = true;
    [Column("stock")]
    public int Stock { get; set; }
    [Column("date_of_buy")]
    public DateTime? DateOfBuy { get; set; }
    [Column("image")]
    public string? Image { get; set; }
    [Column("code_structure")]
    public string? CodeStructure { get; set; }

    // Navigation properties
    public virtual ItemCategory Category { get; set; } = null!;
    public virtual Structure Structure { get; set; } = null!;
    public virtual ICollection<ItemIssue> Issues { get; set; } = [];
    public virtual ICollection<EventSubscription> EventSubscriptions { get; set; } = [];
}
