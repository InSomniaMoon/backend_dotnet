using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Categories;

public class GetItemCategoriesRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }
}
