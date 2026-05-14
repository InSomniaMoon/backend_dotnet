using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class DeleteItemCategoryCommandHandler : IRequestHandler<DeleteItemCategoryCommand, bool>
{
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public DeleteItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
    }

    public async Task<bool> Handle(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var existing = await _itemCategoryRepository.GetByIdAsync(command.Id);
        if (existing is null)
        {
            return false;
        }

        await _itemCategoryRepository.DeleteAsync(command.Id);
        await _itemCategoryRepository.SaveChangesAsync();
        return true;
    }
}
