using GestionMateriel.Application.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Categories;

public class DeleteItemCategoryCommandHandler(GestionMaterielDbContext db)
    : IRequestHandler<DeleteItemCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await db.ItemCategories.FindAsync([command.Id], cancellationToken);
        if (entity is null) return false;

        if (await db.Items.AnyAsync(i => i.CategoryId == command.Id, cancellationToken))
        {
            throw new InvalidOperationException("Vous ne pouvez pas supprimer cette catégorie car elle est utilisée par un ou plusieurs objets.");
        }

        db.ItemCategories.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
