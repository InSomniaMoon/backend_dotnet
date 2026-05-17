namespace GestionMateriel.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using GestionMateriel.Domain.Interfaces;
[Table("events")]
public class Event : BaseEntity, IStructureCodeScopedEntity
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("start_date")]
    public DateTime StartDate { get; set; }
    [Column("end_date")]
    public DateTime EndDate { get; set; }
    [Column("structure_id")]
    public int StructureId { get; set; }
    [Column("user_id")]
    public int? UserId { get; set; }
    [Column("comment")]
    public string? Comment { get; set; }
    [Column("code_structure")]
    public string? CodeStructure { get; set; }

    // Navigation properties
    public virtual Structure Structure { get; set; } = null!;
    public virtual User? CreatedBy { get; set; }
    public virtual ICollection<EventSubscription> Subscriptions { get; set; } = [];
}
