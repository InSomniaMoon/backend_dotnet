using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands.Items;

public class UpdateItemCommandHandler(IItemRepository itemRepository, IMapper mapper) : IRequestHandler<UpdateItemCommand, ItemResponse?>
{
    public async Task<ItemResponse?> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var item = await itemRepository.GetByIdAsync(command.Id);
        if (item is null)
        {
            return null;
        }

        mapper.Map(command.Request, item);

        item.UpdatedAt = DateTime.UtcNow;

        await itemRepository.UpdateAsync(item);
        await itemRepository.SaveChangesAsync();

        return mapper.Map<ItemResponse>(item);
    }
}
