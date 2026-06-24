using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Events;

public class GetEventsByStructureQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetEventsByStructureQuery, IEnumerable<EventResponse>>
{
    public async Task<IEnumerable<EventResponse>> Handle(GetEventsByStructureQuery query,
        CancellationToken cancellationToken)
    {
        var events = await db.Events
            .AsNoTracking()
            .Where(e => e.StartDate >= query.StartDate || e.EndDate >= query.StartDate)
            .Where(e => e.StartDate <= query.EndDate || e.EndDate <= query.EndDate)
            .OrderByDescending(e => e.StartDate)
            .Select(e => new EventResponse()
            {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                StructureId = e.StructureId,
                Comment = e.Comment,
                CreatedById = e.UserId,
            })
            .ToListAsync(cancellationToken);
        return events;
    }
}
