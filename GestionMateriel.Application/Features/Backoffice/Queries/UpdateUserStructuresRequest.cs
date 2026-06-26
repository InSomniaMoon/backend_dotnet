namespace GestionMateriel.Application.Features.Backoffice.Queries;

public class UpdateUserStructuresRequest
{
    public List<UserStructureRequest> Structures { get; set; } = [];
}

public class UserStructureRequest
{
    public int StructureId { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; } = string.Empty;

}