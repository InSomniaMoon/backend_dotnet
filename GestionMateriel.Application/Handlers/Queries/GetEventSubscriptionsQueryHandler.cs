using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetEventSubscriptionsQueryHandler : IRequestHandler<GetEventSubscriptionsQuery, IEnumerable<EventSubscriptionResponse>>
{
    private readonly IEventSubscriptionRepository _eventSubscriptionRepository;
    private readonly IMapper _mapper;

    public GetEventSubscriptionsQueryHandler(IEventSubscriptionRepository eventSubscriptionRepository, IMapper mapper)
    {
        _eventSubscriptionRepository = eventSubscriptionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventSubscriptionResponse>> Handle(GetEventSubscriptionsQuery query, CancellationToken cancellationToken)
    {
        var subscriptions = await _eventSubscriptionRepository.GetByEventAsync(query.EventId);
        return subscriptions.Select(s => _mapper.Map<EventSubscriptionResponse>(s));
    }
}
