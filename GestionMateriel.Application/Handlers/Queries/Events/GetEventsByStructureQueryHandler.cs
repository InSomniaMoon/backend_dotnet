using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Queries.Events;

public class GetEventsByStructureQueryHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<GetEventsByStructureQuery, IEnumerable<EventResponse>>
{
    public async Task<IEnumerable<EventResponse>> Handle(GetEventsByStructureQuery query, CancellationToken cancellationToken)
    {
        var events = await eventRepository.GetEventsByStructureAsync(query.StructureId);
        return events.Select(e => mapper.Map<EventResponse>(e));
    }
}
