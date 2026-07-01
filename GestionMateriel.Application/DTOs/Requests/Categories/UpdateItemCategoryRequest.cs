using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Categories;

public class UpdateItemCategoryRequest
{

    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;

    [Required] public bool Identified { get; set; }
}
