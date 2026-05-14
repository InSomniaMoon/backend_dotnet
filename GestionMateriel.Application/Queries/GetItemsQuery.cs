using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetItemsQuery(int StructureId, int PageNumber = 1, int PageSize = 20)
    : IRequest<PaginatedResponse<ItemResponse>>;
