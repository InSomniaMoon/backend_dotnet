using GestionMateriel.Application.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Events;

public class DeleteEventCommandHandler(GestionMaterielDbContext db)
    : IRequestHandler<DeleteEventCommand, bool>
{
    public async Task<bool> Handle(DeleteEventCommand command, CancellationToken cancellationToken)
    {
        var entity = await db.Events.FindAsync([command.Id], cancellationToken);
        if (entity is null) return false;

        db.Events.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
