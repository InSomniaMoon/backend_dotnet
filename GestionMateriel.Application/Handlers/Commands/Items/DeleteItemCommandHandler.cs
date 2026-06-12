using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Commands.Items;

public class DeleteItemCommandHandler(IItemRepository itemRepository) : IRequestHandler<DeleteItemCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var existingItem = await itemRepository.GetByIdAsync(command.Id);
        if (existingItem is null)
        {
            return false;
        }

        await itemRepository.DeleteAsync(command.Id);
        await itemRepository.SaveChangesAsync();

        return true;
    }
}
