using System.ComponentModel.DataAnnotations.Schema;

namespace GestionMateriel.Domain.Entities;

[Table("item_issue_comments")]
public class ItemIssueComment : BaseEntity
{
    [Column("item_issue_id")]
    public int ItemIssueId { get; set; }
    [Column("comment")]
    public string Comment { get; set; } = string.Empty;
    [Column("user_id")]
    public int UserId { get; set; }

    // Navigation properties
    public virtual ItemIssue ItemIssue { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
