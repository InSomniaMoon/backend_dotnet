using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests;

public class GetEventsByStructureRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }
}
