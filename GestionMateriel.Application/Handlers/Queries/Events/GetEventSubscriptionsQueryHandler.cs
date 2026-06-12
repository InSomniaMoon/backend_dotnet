using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Queries.Events;

public class GetEventSubscriptionsQueryHandler(IEventSubscriptionRepository eventSubscriptionRepository, IMapper mapper) : IRequestHandler<GetEventSubscriptionsQuery, IEnumerable<EventSubscriptionResponse>>
{
    public async Task<IEnumerable<EventSubscriptionResponse>> Handle(GetEventSubscriptionsQuery query, CancellationToken cancellationToken)
    {
        var subscriptions = await eventSubscriptionRepository.GetByEventAsync(query.EventId);
        return subscriptions.Select(s => mapper.Map<EventSubscriptionResponse>(s));
    }
}
