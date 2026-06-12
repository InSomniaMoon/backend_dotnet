using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Events;

public class GetEventSubscriptionsQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetEventSubscriptionsQuery, IEnumerable<EventSubscriptionResponse>>
{
    public async Task<IEnumerable<EventSubscriptionResponse>> Handle(GetEventSubscriptionsQuery query, CancellationToken cancellationToken)
    {
        var subscriptions = await db.EventSubscriptions
            .AsNoTracking()
            .Where(es => es.EventId == query.EventId)
            .OrderBy(es => es.ItemId)
            .ToListAsync(cancellationToken);
        return subscriptions.Select(mapper.Map<EventSubscriptionResponse>);
    }
}
