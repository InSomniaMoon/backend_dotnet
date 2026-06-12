using GestionMateriel.Application.DTOs.Requests.Users;
using GestionMateriel.Application.DTOs.Responses;

namespace GestionMateriel.Application.Commands;

public record CreateUserCommand(CreateUserRequest Request);
