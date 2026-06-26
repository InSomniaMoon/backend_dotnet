using GestionMateriel.Application.DTOs.Requests;

namespace GestionMateriel.Application.Services;

public interface IPasswordResetService
{
    Task SendResetLinkAsync(string email, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
}
