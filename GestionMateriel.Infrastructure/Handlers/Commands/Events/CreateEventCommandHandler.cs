using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Events;

public class CreateEventCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateEventCommand, EventResponse>
{
    public async Task<EventResponse> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        if (command.Request.EndDate < command.Request.StartDate)
            throw new InvalidOperationException("La date de fin doit être supérieure ou égale à la date de début.");


        var structure = await db.Structures.FindAsync([command.Request.StructureId], cancellationToken) ?? throw new InvalidOperationException($"Structure avec l'ID {command.Request.StructureId} introuvable.");

        var itemIds = command.Request.Items.Select(s => s.Id).Distinct().ToList();

        var requestedItems = await db.Items
            .AsNoTracking()
            .Where(i => itemIds.Contains(i.Id))
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Usable,
                IsIdentified = i.Category.Identified
            })
            .ToListAsync(cancellationToken);

        var missingItemIds = itemIds.Except(requestedItems.Select(i => i.Id)).ToList();
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
            .ToList();

        var usedItemNames = identifiedItemIds.Count == 0
            ? []
            : await db.EventSubscriptions
                .AsNoTracking()
                .Where(es => identifiedItemIds.Contains(es.ItemId))
                .Where(es => es.Event.StartDate < command.Request.EndDate)
                .Where(es => es.Event.EndDate > command.Request.StartDate)
                .Select(es => es.Item.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync(cancellationToken);

        if (usedItemNames.Count != 0)
            throw new InvalidOperationException($"Items déjà utilisés sur la période demandée: {string.Join(", ", usedItemNames)}");

        var @event = new Event
        {
            Name = command.Request.Name,
            StartDate = command.Request.StartDate,
            EndDate = command.Request.EndDate,
            CodeStructure = structure.CodeStructure,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Comment = command.Request.Comment,
            StructureId = command.Request.StructureId,
            UserId = command.UserId,
            Subscriptions = [.. command.Request.Items.Select(s => new EventSubscription
            {
                ItemId = s.Id,
                Quantity = s.Quantity,
            })]

        };

        await db.Events.AddAsync(@event, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.Map<EventResponse>(@event);
    }
}
