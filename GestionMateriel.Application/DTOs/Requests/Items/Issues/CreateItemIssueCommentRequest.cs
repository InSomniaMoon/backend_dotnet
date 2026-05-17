using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Items.Issues;

public class CreateItemIssueCommentRequest
{
    [Required]
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
}
