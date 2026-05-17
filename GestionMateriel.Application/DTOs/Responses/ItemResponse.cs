namespace GestionMateriel.Application.DTOs.Responses;

public class ItemResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int StructureId { get; set; }
    public bool Usable { get; set; }
    public int Stock { get; set; }
    public int UsableStock { get; set; }
    public string State { get; set; } = string.Empty;
    public DateTime? DateOfBuy { get; set; }
    public string? ImagePath { get; set; }
    public ItemCategoryResponse Category { get; set; } = null!;
}
