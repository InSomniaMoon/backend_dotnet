using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests;

public class GetItemCategoriesRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }
}
