using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Events;

public class CreateEventCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateEventCommand, EventResponse>
{
    public async Task<EventResponse> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        if (command.Request.EndDate < command.Request.StartDate)
            throw new InvalidOperationException("EndDate must be greater than or equal to StartDate.");

        var entity = mapper.Map<Event>(command.Request);
        await db.Events.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.Map<EventResponse>(entity);
    }
}
