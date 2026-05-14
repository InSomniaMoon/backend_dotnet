using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetUserStructuresQuery(int UserId) : IRequest<UserWithStructuresResponse?>;
