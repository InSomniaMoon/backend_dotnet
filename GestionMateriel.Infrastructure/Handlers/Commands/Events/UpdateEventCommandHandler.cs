using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Events;

public class UpdateEventCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<UpdateEventCommand, EventResponse?>
{
    public async Task<EventResponse?> Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {
        if (command.Request.EndDate < command.Request.StartDate)
            throw new InvalidOperationException("EndDate must be greater than or equal to StartDate.");

        var entity = await db.Events
            .Include(e => e.Subscriptions)
            .FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);
        if (entity is null) return null;

        var requestedByItemId = command.Request.Items
            .GroupBy(i => i.Id)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

        var requestedItemIds = requestedByItemId.Keys.ToList();

        var requestedItems = await db.Items
            .AsNoTracking()
            .Where(i => requestedItemIds.Contains(i.Id))
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Usable,
                i.UsableStock,
                IsIdentified = i.Category.Identified
            })
            .ToListAsync(cancellationToken);

        var missingItemIds = requestedItemIds.Except(requestedItems.Select(i => i.Id)).ToList();
        if (missingItemIds.Count != 0)
            throw new InvalidOperationException($"Items introuvables: {string.Join(", ", missingItemIds)}");

        var unusableItemNames = requestedItems
            .Where(i => !i.Usable)
            .Select(i => i.Name)
            .Distinct()
            .OrderBy(n => n)
            .ToList();

        if (unusableItemNames.Count != 0)
            throw new InvalidOperationException($"Items non utilisables pour cet événement: {string.Join(", ", unusableItemNames)}");

        var identifiedItemIds = requestedItems
            .Where(i => i.IsIdentified)
            .Select(i => i.Id)
            .Distinct()
            .ToList();

        if (identifiedItemIds.Count != 0)
        {
            var conflictingItemNames = await db.EventSubscriptions
                .AsNoTracking()
                .Where(es => es.EventId != command.Id)
                .Where(es => identifiedItemIds.Contains(es.ItemId))
                .Where(es => es.Event.StartDate < command.Request.EndDate)
                .Where(es => es.Event.EndDate > command.Request.StartDate)
                .Select(es => es.Item.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync(cancellationToken);

            if (conflictingItemNames.Count != 0)
                throw new InvalidOperationException($"Items déjà utilisés sur la période demandée: {string.Join(", ", conflictingItemNames)}");
        }

        var nonIdentifiedSubscriptions = requestedItems
            .Where(i => !i.IsIdentified)
            .Select(i => new
            {
                ItemId = i.Id,
                ItemName = i.Name,
                Quantity = requestedByItemId[i.Id],
                i.UsableStock
            })
            .ToList();

        if (nonIdentifiedSubscriptions.Count != 0)
        {
            var nonIdentifiedItemIds = nonIdentifiedSubscriptions
                .Select(es => es.ItemId)
                .Distinct()
                .ToList();

            var usedQuantities = await db.EventSubscriptions
                .AsNoTracking()
                .Where(es => es.EventId != command.Id)
                .Where(es => nonIdentifiedItemIds.Contains(es.ItemId))
                .Where(es => es.Event.StartDate < command.Request.EndDate)
                .Where(es => es.Event.EndDate > command.Request.StartDate)
                .GroupBy(es => es.ItemId)
                .Select(g => new { ItemId = g.Key, UsedQuantity = g.Sum(es => es.Quantity) })
                .ToDictionaryAsync(x => x.ItemId, x => x.UsedQuantity, cancellationToken);

            var quantityConflicts = nonIdentifiedSubscriptions
                .Where(es =>
                {
                    var usedElsewhere = usedQuantities.TryGetValue(es.ItemId, out var used) ? used : 0;
                    return usedElsewhere + es.Quantity > es.UsableStock;
                })
                .Select(es =>
                {
                    var usedElsewhere = usedQuantities.TryGetValue(es.ItemId, out var used) ? used : 0;
                    return $"{es.ItemName} (demandé: {es.Quantity}, déjà utilisé: {usedElsewhere}, stock utilisable: {es.UsableStock})";
                })
                .OrderBy(x => x)
                .ToList();

            if (quantityConflicts.Count != 0)
                throw new InvalidOperationException($"Quantités indisponibles sur la période demandée: {string.Join("; ", quantityConflicts)}");
        }

        entity.Name = command.Request.Name;
        entity.StartDate = command.Request.StartDate;
        entity.EndDate = command.Request.EndDate;
        entity.Comment = command.Request.Comment;
        entity.StructureId = command.Request.StructureId;

        var subscriptionsToRemove = entity.Subscriptions
            .Where(es => !requestedByItemId.ContainsKey(es.ItemId))
            .ToList();

        if (subscriptionsToRemove.Count != 0)
        {
            db.EventSubscriptions.RemoveRange(subscriptionsToRemove);
        }

        var existingByItemId = entity.Subscriptions.ToDictionary(es => es.ItemId);
        foreach (var (itemId, quantity) in requestedByItemId)
        {
            if (existingByItemId.TryGetValue(itemId, out var existingSubscription))
            {
                existingSubscription.Quantity = quantity;
            }
            else
            {
                entity.Subscriptions.Add(new EventSubscription
                {
                    EventId = command.Id,
                    ItemId = itemId,
                    Quantity = quantity
                });
            }
        }

        entity.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<EventResponse>(entity);
    }
}
