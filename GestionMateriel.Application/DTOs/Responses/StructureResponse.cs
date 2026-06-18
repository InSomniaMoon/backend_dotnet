using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Application.DTOs.Responses;

public class StructureResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CodeStructure { get; set; } = string.Empty;
    public string NomStructure { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? ParentCode { get; set; }
    public string? Color { get; set; }
    public string? ImagePath { get; set; }
}

public class StructureWithRoleResponse : StructureResponse
{
    public string Role { get; set; } = RoleEnum.User.ToString();
}

public class StructureWithMembersResponse : StructureResponse
{
    public List<UserResponse> Members { get; set; } = [];
}