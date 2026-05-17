using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetItemsQuery(
    int Page = 1,
    int Size = 20,
    string? Q = null,
    string? SortDir = "asc",
    string? SortBy = null
    )
    : IRequest<PaginatedResponse<ItemResponse>>;
