using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests;

public class UpdateEventRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }

    [Range(1, int.MaxValue)]
    public int? CreatedById { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }
}
