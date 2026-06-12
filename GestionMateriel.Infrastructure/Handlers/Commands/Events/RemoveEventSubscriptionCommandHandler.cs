using GestionMateriel.Application.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Events;

public class RemoveEventSubscriptionCommandHandler(GestionMaterielDbContext db)
    : IRequestHandler<RemoveEventSubscriptionCommand, bool>
{
    public async Task<bool> Handle(RemoveEventSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var existing = await db.EventSubscriptions
            .FirstOrDefaultAsync(es => es.EventId == command.EventId && es.ItemId == command.ItemId, cancellationToken);
        if (existing is null) return false;

        db.EventSubscriptions.Remove(existing);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
