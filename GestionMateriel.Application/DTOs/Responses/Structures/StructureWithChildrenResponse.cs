namespace GestionMateriel.Application.DTOs.Responses.Structures;

public class StructureWithChildrenResponse
{
    public StructureWithMembersResponse Structure { get; set; } = null!;
    public List<StructureWithMembersResponse> Children { get; set; } = [];
}