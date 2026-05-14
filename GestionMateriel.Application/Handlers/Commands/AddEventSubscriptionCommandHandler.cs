using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class AddEventSubscriptionCommandHandler : IRequestHandler<AddEventSubscriptionCommand, EventSubscriptionResponse?>
{
    private readonly IEventRepository _eventRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IEventSubscriptionRepository _eventSubscriptionRepository;
    private readonly IMapper _mapper;

    public AddEventSubscriptionCommandHandler(
        IEventRepository eventRepository,
        IItemRepository itemRepository,
        IEventSubscriptionRepository eventSubscriptionRepository,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _itemRepository = itemRepository;
        _eventSubscriptionRepository = eventSubscriptionRepository;
        _mapper = mapper;
    }

    public async Task<EventSubscriptionResponse?> Handle(AddEventSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(command.EventId);
        if (eventEntity is null)
        {
            return null;
        }

        var itemEntity = await _itemRepository.GetByIdAsync(command.Request.ItemId);
        if (itemEntity is null)
        {
            return null;
        }

        var existing = await _eventSubscriptionRepository.GetAsync(command.EventId, command.Request.ItemId);
        if (existing is not null)
        {
            existing.Quantity = command.Request.Quantity;
            await _eventSubscriptionRepository.SaveChangesAsync();
            return _mapper.Map<EventSubscriptionResponse>(existing);
        }

        var subscription = new EventSubscription
        {
            EventId = command.EventId,
            ItemId = command.Request.ItemId,
            Quantity = command.Request.Quantity
        };

        await _eventSubscriptionRepository.AddAsync(subscription);
        await _eventSubscriptionRepository.SaveChangesAsync();

        return _mapper.Map<EventSubscriptionResponse>(subscription);
    }
}
