using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests;

public class CreateItemRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }

    public bool Usable { get; set; } = true;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public DateTime? DateOfBuy { get; set; }

    [MaxLength(500)]
    public string? ImagePath { get; set; }
}
