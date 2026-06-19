using System.ComponentModel.DataAnnotations.Schema;
using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Domain.Entities;

[Table("users")]
public class User : BaseEntity
{
    [Column("firstname")] public string FirstName { get; set; } = string.Empty;
    [Column("lastname")] public string LastName { get; set; } = string.Empty;
    [Column("email")] public string Email { get; set; } = string.Empty;
    [Column("password")] public string Password { get; set; } = string.Empty;
    [Column("phone")] public string? Phone { get; set; }
    [Column("role")] public RoleEnum Role { get; set; } = RoleEnum.User;

    // Navigation properties
    public virtual ICollection<UserStructure> UserStructures { get; set; } = [];
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public virtual ICollection<ItemIssue> ReportedIssues { get; set; } = [];
    public virtual ICollection<ItemIssueComment> Comments { get; set; } = [];
    public virtual ICollection<Event> CreatedEvents { get; set; } = [];
    public virtual ICollection<FeatureClick> FeatureClicks { get; set; } = [];
}
