using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Items.Issues;

public class CreateItemIssueRequest
{

    [Required]
    [MaxLength(500)]
    public required string Value { get; set; }

    [Range(1, int.MaxValue)]
    public required int AffectedQuantity { get; set; }

    [Required]
    public required bool Usable { get; set; }
}
