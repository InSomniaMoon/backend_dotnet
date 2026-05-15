using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetItemByIdQueryHandler(IItemRepository itemRepository, IMapper mapper) : IRequestHandler<GetItemByIdQuery, ItemResponse?>
{
    public async Task<ItemResponse?> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await itemRepository.GetByIdAsync(query.Id);
        return item is null ? null : mapper.Map<ItemResponse>(item);
    }
}
