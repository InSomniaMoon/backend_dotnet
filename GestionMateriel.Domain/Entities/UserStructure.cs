using System.ComponentModel.DataAnnotations.Schema;
using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Domain.Entities;

[Table("user_structures")]
public class UserStructure
{
    // Composite key
    [Column("user_id")]
    public int UserId { get; set; }
    [Column("structure_id")]
    public int StructureId { get; set; }
    [Column("role")]
    public RoleEnum Role { get; set; } = RoleEnum.User;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Structure Structure { get; set; } = null!;

}
