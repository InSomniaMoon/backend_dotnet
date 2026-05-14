using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests;

public class CreateItemCategoryRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }

    public bool Identified { get; set; }
}
