using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetUsersQuery() : IRequest<IEnumerable<UserResponse>>;
