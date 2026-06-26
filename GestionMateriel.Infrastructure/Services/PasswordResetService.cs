using System.Security.Cryptography;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.Services;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GestionMateriel.Infrastructure.Services;

public class PasswordResetService(
    GestionMaterielDbContext db,
    IEmailService emailService,
    IOptions<PasswordResetSettings> passwordResetOptions,
    IOptions<FrontendSettings> frontendOptions) : IPasswordResetService
{
    private readonly PasswordResetSettings _passwordResetSettings = passwordResetOptions.Value;
    private readonly FrontendSettings _frontendSettings = frontendOptions.Value;

    public async Task SendResetLinkAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user is null)
            return;

        var existingToken = await db.PasswordResetTokens.FirstOrDefaultAsync(t => t.Email == email, cancellationToken);
        if (existingToken is not null &&
            (DateTime.UtcNow - existingToken.CreatedAt).TotalSeconds < _passwordResetSettings.ThrottleSeconds)
        {
            return;
        }

        var plainToken = GenerateToken();
        var hashedToken = HashToken(plainToken);

        if (existingToken is not null)
        {
            db.PasswordResetTokens.Remove(existingToken);
        }

        await db.PasswordResetTokens.AddAsync(new PasswordResetToken
        {
            Email = email,
            Token = hashedToken,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        var resetUrl = $"{_frontendSettings.BaseUrl.TrimEnd('/')}/auth/reset-password?token={Uri.EscapeDataString(plainToken)}";

        await emailService.SendAsync(
            user.Email,
            $"{user.FirstName} {user.LastName}",
            "Réinitialisation de votre mot de passe",
            $"""
             <p>Bonjour {user.FirstName},</p>
             <p>Vous avez demandé la réinitialisation de votre mot de passe.</p>
             <p><a href="{resetUrl}">Cliquez ici pour choisir un nouveau mot de passe</a></p>
             <p>Ce lien est valable {_passwordResetSettings.ExpireMinutes} minutes. Si vous n'êtes pas à l'origine de cette demande, vous pouvez ignorer cet email.</p>
             """,
            cancellationToken);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var token = await db.PasswordResetTokens.FirstOrDefaultAsync(t => t.Email == request.Email, cancellationToken);
        if (token is null || token.Token != HashToken(request.Token))
            throw new InvalidOperationException("Ce lien de réinitialisation est invalide.");

        if ((DateTime.UtcNow - token.CreatedAt).TotalMinutes > _passwordResetSettings.ExpireMinutes)
            throw new InvalidOperationException("Ce lien de réinitialisation a expiré.");

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user is null)
            throw new InvalidOperationException("Ce lien de réinitialisation est invalide.");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        db.PasswordResetTokens.Remove(token);
        await db.SaveChangesAsync(cancellationToken);
    }

    private static string GenerateToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    private static string HashToken(string token) => Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(token)));
}
