namespace GestionMateriel.Infrastructure.Services;

public record JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
    public string Role { get; set; } = string.Empty;
    public StructureSettings Structure { get; set; } = new();
}

public record StructureSettings
{
    public string Mask { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}