using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Events;

public class GetEventByIdQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetEventByIdQuery, EventResponse?>
{
    public async Task<EventResponse?> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await db.Events.FindAsync([query.Id], cancellationToken);
        return entity is null ? null : mapper.Map<EventResponse>(entity);
    }
}
