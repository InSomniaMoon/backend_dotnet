using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Items.Issues;

public class CreateItemIssueRequest
{
    [Range(1, int.MaxValue)]
    public int ItemId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Value { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ReportedById { get; set; }

    [Range(1, int.MaxValue)]
    public int AffectedQuantity { get; set; } = 1;
}
