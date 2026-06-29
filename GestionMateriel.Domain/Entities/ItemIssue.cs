using System.ComponentModel.DataAnnotations.Schema;
using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Domain.Entities;

[Table("item_issues")]
public class ItemIssue : BaseEntity
{
    [Column("item_id")]
    public int ItemId { get; set; }
    [Column("status")]
    public IssueStatusEnum Status { get; set; } = IssueStatusEnum.Open;
    [Column("value")]
    public string Value { get; set; } = string.Empty;
    [Column("reported_by")]
    public int ReportedBy { get; set; }
    [Column("affected_quantity")]
    public int AffectedQuantity { get; set; }
    [Column("resolution_date")]
    public DateTime? ResolutionDate { get; set; }
    [Column("is_item_usable")]
    public bool IsItemUsable { get; set; } = false;

    // Navigation properties
    public virtual Item Item { get; set; } = null!;
    public virtual User ReportedByUser { get; set; } = null!;
    public virtual ICollection<ItemIssueComment> Comments { get; set; } = [];
}
