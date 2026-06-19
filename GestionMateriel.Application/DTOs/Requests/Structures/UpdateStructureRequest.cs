using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Structures;

public class UpdateStructureRequest
{
    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [Required] [MaxLength(255)] public string NomStructure { get; set; } = string.Empty;

    [Required] public string Type { get; set; } = string.Empty;

    [MaxLength(50)] public string? ParentCode { get; set; }

    [MaxLength(7)] public string? Color { get; set; }

    [MaxLength(500)] public string? Image { get; set; }
}
