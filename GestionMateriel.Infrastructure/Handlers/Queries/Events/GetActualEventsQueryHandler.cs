using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Events;

public class GetActualEventsQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetActualEventsQuery, IEnumerable<EventResponse>>
{
    public async Task<IEnumerable<EventResponse>> Handle(GetActualEventsQuery query, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var events = await db.Events
            .AsNoTracking()
            .Where(e => e.EndDate >= now)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
        return events.Select(mapper.Map<EventResponse>);
    }
}
