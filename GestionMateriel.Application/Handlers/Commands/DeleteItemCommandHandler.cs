using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
{
    private readonly IItemRepository _itemRepository;

    public DeleteItemCommandHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<bool> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var existingItem = await _itemRepository.GetByIdAsync(command.Id);
        if (existingItem is null)
        {
            return false;
        }

        await _itemRepository.DeleteAsync(command.Id);
        await _itemRepository.SaveChangesAsync();

        return true;
    }
}
