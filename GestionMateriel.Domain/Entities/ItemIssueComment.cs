namespace GestionMateriel.Domain.Entities;

public class ItemIssueComment : BaseEntity
{
    public int ItemIssueId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int UserId { get; set; }
    
    // Navigation properties
    public ItemIssue ItemIssue { get; set; } = null!;
    public User User { get; set; } = null!;
}
