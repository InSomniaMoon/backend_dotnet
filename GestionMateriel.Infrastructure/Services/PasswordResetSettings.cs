namespace GestionMateriel.Infrastructure.Services;

public record PasswordResetSettings
{
    public int ExpireMinutes { get; set; } = 60;
    public int ThrottleSeconds { get; set; } = 60;
}
