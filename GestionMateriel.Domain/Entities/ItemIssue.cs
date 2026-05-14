using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Domain.Entities;

public class ItemIssue : BaseEntity
{
    public int ItemId { get; set; }
    public IssueStatusEnum Status { get; set; } = IssueStatusEnum.Open;
    public string Value { get; set; } = string.Empty;
    public int ReportedById { get; set; }
    public int AffectedQuantity { get; set; }
    public DateTime? ResolutionDate { get; set; }

    // Navigation properties
    public Item Item { get; set; } = null!;
    public User ReportedBy { get; set; } = null!;
    public ICollection<ItemIssueComment> Comments { get; set; } = [];
}
