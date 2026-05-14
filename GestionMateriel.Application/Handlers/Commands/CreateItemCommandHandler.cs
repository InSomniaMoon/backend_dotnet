using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemResponse>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public CreateItemCommandHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<ItemResponse> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Domain.Entities.Item>(command.Request);

        await _itemRepository.AddAsync(item);
        await _itemRepository.SaveChangesAsync();

        return _mapper.Map<ItemResponse>(item);
    }
}
