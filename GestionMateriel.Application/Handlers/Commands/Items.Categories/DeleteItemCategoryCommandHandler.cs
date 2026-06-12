using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Commands.Items.Categories;

public class DeleteItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository) : IRequestHandler<DeleteItemCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var existing = await itemCategoryRepository.GetByIdAsync(command.Id);
        if (existing is null)
        {
            return false;
        }

        await itemCategoryRepository.DeleteAsync(command.Id);
        await itemCategoryRepository.SaveChangesAsync();
        return true;
    }
}
