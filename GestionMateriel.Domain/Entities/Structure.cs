using System.ComponentModel.DataAnnotations.Schema;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;

namespace GestionMateriel.Domain.Entities;

[Table("structures")]
public class Structure : IStructureCodeScopedEntity
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("code_structure")]
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public string CodeStructure { get; set; } = string.Empty;
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    [Column("nom_structure")]
    public string NomStructure { get; set; } = string.Empty;
    [Column("type")]
    public StructureTypeEnum Type { get; set; } = StructureTypeEnum.Unite;
    [Column("parent_code")]
    public string? ParentCode { get; set; }
    [Column("color")]
    public string? Color { get; set; }
    [Column("image")]
    public string? Image { get; set; }

    // Navigation properties
    public virtual ICollection<UserStructure> UserStructures { get; set; } = [];
    public virtual ICollection<Item> Items { get; set; } = [];
    public virtual ICollection<ItemCategory> ItemCategories { get; set; } = [];
    public virtual ICollection<Event> Events { get; set; } = [];
}
