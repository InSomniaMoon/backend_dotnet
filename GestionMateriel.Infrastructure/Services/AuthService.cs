using BCrypt.Net;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Services;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace GestionMateriel.Infrastructure.Services;

public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService,
    IOptions<JwtSettings> jwtOptions) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return null;
        }

        return await BuildAuthResponseAsync(user, user.UserStructures.FirstOrDefault()?.Structure);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await userRepository.IsEmailTakenAsync(request.Email))
        {
            throw new InvalidOperationException("An account already exists with this email.");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = RoleEnum.User
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return await BuildAuthResponseAsync(user, user.UserStructures.FirstOrDefault()?.Structure);
    }

    public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var storedRefreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (storedRefreshToken is null || storedRefreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        var user = storedRefreshToken.User;

        var userstructure = user.UserStructures.FirstOrDefault()?.Structure;

        await refreshTokenRepository.DeleteAsync(storedRefreshToken.Id);
        await refreshTokenRepository.SaveChangesAsync();

        return await BuildAuthResponseAsync(user, userstructure);
    }

    private async Task<AuthResponse> BuildAuthResponseAsync(User user, Structure? selectedStructure = null)
    {
        var (accessToken, expiresAtUtc) = jwtTokenService.GenerateAccessToken(user, selectedStructure);
        var refreshTokenValue = jwtTokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        };

        await refreshTokenRepository.AddAsync(refreshToken);
        await refreshTokenRepository.SaveChangesAsync();

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
