using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GestionMateriel.Application.Services;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GestionMateriel.Infrastructure.Services;

public class JwtTokenService(IOptions<JwtSettings> options) : IJwtTokenService
{
    private readonly JwtSettings _settings = options.Value;

    public (string accessToken, DateTime expiresAtUtc) GenerateAccessToken(User user, Structure? selectedStructure)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //   $code_mask = match ($structure?->type) {
        //       Structure::GROUPE, Structure::UNITE => substr($structure?->code_structure, 0, -2),

        //       Structure::TERRITOIRE => substr($structure?->code_structure, 0, -4),
        //       default => $structure?->code_structure,
        //     };
        var structureMask = selectedStructure?.Type switch
        {
            var t when t == StructureTypeEnum.Unite || t == StructureTypeEnum.Groupe => selectedStructure?.CodeStructure[..^2],
            var t when t == StructureTypeEnum.Territoire => selectedStructure?.CodeStructure[..^4],
            _ => selectedStructure?.CodeStructure
        };


        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("firstName", user.FirstName),
            new("lastName", user.LastName),
            new("structureId", selectedStructure?.Id.ToString() ?? string.Empty),
            new("structureType", selectedStructure?.Type.ToString() ?? string.Empty),
            new("structureCode", selectedStructure?.CodeStructure ?? string.Empty),
            new("structureMask", structureMask ?? string.Empty),
        };

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
}
