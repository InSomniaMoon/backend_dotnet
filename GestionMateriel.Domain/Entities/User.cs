using System.ComponentModel.DataAnnotations.Schema;
using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Domain.Entities;

[Table("users")]
public class User : BaseEntity
{
    [Column("firstname")]
    public string FirstName { get; set; } = string.Empty;
    [Column("lastname")]
    public string LastName { get; set; } = string.Empty;
    [Column("email")]
    public string Email { get; set; } = string.Empty;
    [Column("password")]
    public string Password { get; set; } = string.Empty;
    [Column("phone")]
    public string? Phone { get; set; }
    [Column("role")]
    public RoleEnum Role { get; set; } = RoleEnum.User;

    // Navigation properties
    public ICollection<UserStructure> UserStructures { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<ItemIssue> ReportedIssues { get; set; } = [];
    public ICollection<ItemIssueComment> Comments { get; set; } = [];
    public ICollection<Event> CreatedEvents { get; set; } = [];
    public ICollection<FeatureClick> FeatureClicks { get; set; } = [];
}
