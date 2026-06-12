using GestionMateriel.Application.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Categories;

public class DeleteItemCategoryCommandHandler(GestionMaterielDbContext db)
    : IRequestHandler<DeleteItemCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await db.ItemCategories.FindAsync([command.Id], cancellationToken);
        if (entity is null) return false;

        db.ItemCategories.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
