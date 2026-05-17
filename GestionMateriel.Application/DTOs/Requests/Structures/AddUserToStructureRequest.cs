using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Structures;

public class AddUserToStructureRequest
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;
}
