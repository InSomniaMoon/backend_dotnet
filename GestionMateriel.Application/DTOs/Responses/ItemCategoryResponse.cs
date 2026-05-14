namespace GestionMateriel.Application.DTOs.Responses;

public class ItemCategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StructureId { get; set; }
    public bool Identified { get; set; }
}
