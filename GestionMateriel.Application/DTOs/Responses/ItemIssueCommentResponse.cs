namespace GestionMateriel.Application.DTOs.Responses;

public class ItemIssueCommentResponse
{
    public int Id { get; set; }
    public int ItemIssueId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
