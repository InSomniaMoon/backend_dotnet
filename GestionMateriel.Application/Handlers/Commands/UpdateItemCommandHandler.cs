using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemResponse?>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public UpdateItemCommandHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<ItemResponse?> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetByIdAsync(command.Id);
        if (item is null)
        {
            return null;
        }

        _mapper.Map(command.Request, item);
        item.UpdatedAt = DateTime.UtcNow;

        await _itemRepository.UpdateAsync(item);
        await _itemRepository.SaveChangesAsync();

        return _mapper.Map<ItemResponse>(item);
    }
}
