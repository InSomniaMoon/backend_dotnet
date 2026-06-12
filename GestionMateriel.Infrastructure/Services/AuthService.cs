using BCrypt.Net;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Services;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GestionMateriel.Infrastructure.Services;

public class AuthService(
    GestionMaterielDbContext db,
    IJwtTokenService jwtTokenService,
    IOptions<JwtSettings> jwtOptions) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await db.Users
            .AsNoTracking()
            .AsSplitQuery()
            .Include(u => u.UserStructures)
                .ThenInclude(us => us.Structure)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return null;

        return await BuildAuthResponseAsync(user, user.UserStructures.FirstOrDefault()?.Structure, cancellationToken);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new InvalidOperationException("An account already exists with this email.");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = RoleEnum.User
        };

        await db.Users.AddAsync(user, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return await BuildAuthResponseAsync(user, null, cancellationToken);
    }

    public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var storedToken = await db.RefreshTokens
            .Include(rt => rt.User)
                .ThenInclude(u => u.UserStructures)
                .ThenInclude(us => us.Structure)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (storedToken is null || storedToken.ExpiresAt <= DateTime.UtcNow)
            return null;

        var user = storedToken.User;
        var structure = user.UserStructures.FirstOrDefault()?.Structure;

        db.RefreshTokens.Remove(storedToken);
        await db.SaveChangesAsync(cancellationToken);

        return await BuildAuthResponseAsync(user, structure, cancellationToken);
    }

    private async Task<AuthResponse> BuildAuthResponseAsync(User user, Structure? selectedStructure, CancellationToken cancellationToken = default)
    {
        var (accessToken, expiresAtUtc) = jwtTokenService.GenerateAccessToken(user, selectedStructure);
        var refreshTokenValue = jwtTokenService.GenerateRefreshToken();

        await db.RefreshTokens.AddAsync(new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        }, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            Token = accessToken,
            RefreshToken = refreshTokenValue,
            ExpiresAtUtc = expiresAtUtc,
            User = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role.ToString()
            },
            Structures = [.. user.UserStructures.Select(s => new StructureWithRoleResponse
            {
                Id = s.StructureId,
                Name = s.Structure.Name,
                CodeStructure = s.Structure.CodeStructure,
                NomStructure = s.Structure.NomStructure,
                Type = s.Structure.Type.ToString(),
                ParentCode = s.Structure.ParentCode,
                Color = s.Structure.Color,
                ImagePath = s.Structure.Image,
                Role = s.Role.ToString()
            })]
        };
    }
}
