using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Categories;

public class CreateItemCategoryRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public bool Identified { get; set; }
}
