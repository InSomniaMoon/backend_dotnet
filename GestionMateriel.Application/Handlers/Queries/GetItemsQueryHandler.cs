using AutoMapper;
using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetItemsQueryHandler(IItemRepository itemRepository, IMapper mapper) : IRequestHandler<GetItemsQuery, PaginatedResponse<ItemResponse>>
{
    public async Task<PaginatedResponse<ItemResponse>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var items = (await itemRepository.GetByStructureAsync(request.StructureId)).ToList();
        var totalCount = items.Count;

        var pagedItems = items
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(item => mapper.Map<ItemResponse>(item))
            .ToList();

        return new PaginatedResponse<ItemResponse>
        {
            Data = pagedItems,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
