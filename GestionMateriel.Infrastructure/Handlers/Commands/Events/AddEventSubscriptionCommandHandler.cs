using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Events;

public class AddEventSubscriptionCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<AddEventSubscriptionCommand, EventSubscriptionResponse?>
{
    public async Task<EventSubscriptionResponse?> Handle(AddEventSubscriptionCommand command, CancellationToken cancellationToken)
    {

        var eventEntity = await db.Events.FindAsync([command.EventId], cancellationToken);
        if (eventEntity is null) return null;

        var itemEntity = await db.Items.FindAsync([command.Request.ItemId], cancellationToken);
        if (itemEntity is null) return null;

        var existing = await db.EventSubscriptions
            .FirstOrDefaultAsync(es => es.EventId == command.EventId && es.ItemId == command.Request.ItemId, cancellationToken);

        if (existing is not null)
        {
            existing.Quantity = command.Request.Quantity;
            await db.SaveChangesAsync(cancellationToken);
            return mapper.Map<EventSubscriptionResponse>(existing);
        }

        var subscription = new EventSubscription
        {
            EventId = command.EventId,
            ItemId = command.Request.ItemId,
            Quantity = command.Request.Quantity
        };

        await db.EventSubscriptions.AddAsync(subscription, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.Map<EventSubscriptionResponse>(subscription);
    }
}
