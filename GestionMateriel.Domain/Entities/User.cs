using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public RoleEnum Role { get; set; } = RoleEnum.User;

    // Navigation properties
    public ICollection<UserStructure> UserStructures { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<ItemIssue> ReportedIssues { get; set; } = [];
    public ICollection<ItemIssueComment> Comments { get; set; } = [];
    public ICollection<Event> CreatedEvents { get; set; } = [];
    public ICollection<FeatureClick> FeatureClicks { get; set; } = [];
}
