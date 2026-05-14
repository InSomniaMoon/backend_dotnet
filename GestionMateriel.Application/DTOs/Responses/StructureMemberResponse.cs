namespace GestionMateriel.Application.DTOs.Responses;

public class StructureMemberResponse
{
    public int UserId { get; set; }
    public int StructureId { get; set; }
    public string Role { get; set; } = string.Empty;
}
