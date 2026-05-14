using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Domain.Entities;

public class UserStructure
{
    // Composite key
    public int UserId { get; set; }
    public int StructureId { get; set; }
    public RoleEnum Role { get; set; } = RoleEnum.User;
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Structure Structure { get; set; } = null!;
}
