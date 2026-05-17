using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Items;

public class GetItemsRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int StructureId { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
}
