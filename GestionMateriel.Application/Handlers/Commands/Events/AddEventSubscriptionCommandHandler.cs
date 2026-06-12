using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Commands.Events;

public class AddEventSubscriptionCommandHandler(
    IEventRepository eventRepository,
    IItemRepository itemRepository,
    IEventSubscriptionRepository eventSubscriptionRepository,
    IMapper mapper) : IRequestHandler<AddEventSubscriptionCommand, EventSubscriptionResponse?>
{
    public async Task<EventSubscriptionResponse?> Handle(AddEventSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await eventRepository.GetByIdAsync(command.EventId);
        if (eventEntity is null)
        {
            return null;
        }

        var itemEntity = await itemRepository.GetByIdAsync(command.Request.ItemId);
        if (itemEntity is null)
        {
            return null;
        }

        var existing = await eventSubscriptionRepository.GetAsync(command.EventId, command.Request.ItemId);
        if (existing is not null)
        {
            existing.Quantity = command.Request.Quantity;
            await eventSubscriptionRepository.SaveChangesAsync();
            return mapper.Map<EventSubscriptionResponse>(existing);
        }

        var subscription = new EventSubscription
        {
            EventId = command.EventId,
            ItemId = command.Request.ItemId,
            Quantity = command.Request.Quantity
        };

        await eventSubscriptionRepository.AddAsync(subscription);
        await eventSubscriptionRepository.SaveChangesAsync();

        return mapper.Map<EventSubscriptionResponse>(subscription);
    }
}
