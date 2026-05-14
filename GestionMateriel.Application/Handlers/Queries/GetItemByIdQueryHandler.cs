using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemResponse?>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public GetItemByIdQueryHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<ItemResponse?> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetByIdAsync(query.Id);
        return item is null ? null : _mapper.Map<ItemResponse>(item);
    }
}
