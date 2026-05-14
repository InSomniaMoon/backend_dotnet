namespace GestionMateriel.Application.DTOs.Responses;

public class ItemIssueResponse
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int ReportedById { get; set; }
    public int AffectedQuantity { get; set; }
    public DateTime? ResolutionDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
