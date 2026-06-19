using GestionMateriel.Application.Features.Items.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items;

public class DeleteItemCommandHandler(GestionMaterielDbContext db)
    : IRequestHandler<DeleteItemCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var item = await db.Items.FindAsync([command.Id], cancellationToken);
        if (item is null) return false;

        db.Items.Remove(item);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
