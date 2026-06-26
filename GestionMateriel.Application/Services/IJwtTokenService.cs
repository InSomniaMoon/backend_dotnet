using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Application.Services;

public interface IJwtTokenService
{
    (string accessToken, DateTime expiresAtUtc) GenerateAccessToken(User user, RoleEnum structureRole, Structure? selectedStructure = null);
    string GenerateRefreshToken();
}
