using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Categories;

public class UpdateItemCategoryRequest
{
    [Required] public bool Usable { get; set; }

    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [MaxLength(1000)] public string? Description { get; set; } = string.Empty;

    [Required] public int CategoryId { get; set; }

    public DateTime? DateOfBuy { get; set; }

    public string? Image { get; set; }

    [Required] public int Stock { get; set; }
}
