using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetItemByIdQuery(int Id) : IRequest<ItemResponse?>;
