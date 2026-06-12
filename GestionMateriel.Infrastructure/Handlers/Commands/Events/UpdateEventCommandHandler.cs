using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Events;

public class UpdateEventCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<UpdateEventCommand, EventResponse?>
{
    public async Task<EventResponse?> Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {
        if (command.Request.EndDate < command.Request.StartDate)
            throw new InvalidOperationException("EndDate must be greater than or equal to StartDate.");

        var entity = await db.Events.FindAsync([command.Id], cancellationToken);
        if (entity is null) return null;

        mapper.Map(command.Request, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<EventResponse>(entity);
    }
}
