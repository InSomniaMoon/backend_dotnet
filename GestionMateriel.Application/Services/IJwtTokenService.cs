using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Application.Services;

public interface IJwtTokenService
{
    (string accessToken, DateTime expiresAtUtc) GenerateAccessToken(User user, Structure? selectedStructure = null);
    string GenerateRefreshToken();
}
