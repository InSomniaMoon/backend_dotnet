namespace GestionMateriel.Application.DTOs.Responses;

public class UserWithStructuresResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Role { get; set; } = string.Empty;
    public IEnumerable<StructureMemberResponse> Structures { get; set; } = [];
}
