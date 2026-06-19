using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Structures;

public class UpdateStructureRequest
{
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;

    [MaxLength(7)] public string? Color { get; set; }

    [MaxLength(500)] public string? Image { get; set; }

    public List<int> Members { get; set; } = [];
}
