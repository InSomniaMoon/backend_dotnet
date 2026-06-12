using AutoMapper;
using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Queries.Items;

public class GetItemsQueryHandler(IItemRepository itemRepository, IMapper mapper) : IRequestHandler<GetItemsQuery, PaginatedResponse<ItemResponse>>
{
    public async Task<PaginatedResponse<ItemResponse>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await itemRepository.GetByStructureAsync(request.Page, request.Size, request.Q, request.OrderBy, request.OrderDir, cancellationToken);

        var pagedItems = items
            .Select(mapper.Map<ItemResponse>)
            .ToList();

        return new PaginatedResponse<ItemResponse>
        {
            Data = pagedItems,
            TotalCount = totalCount,
            Page = request.Page,
            Size = request.Size
        };
    }
}
