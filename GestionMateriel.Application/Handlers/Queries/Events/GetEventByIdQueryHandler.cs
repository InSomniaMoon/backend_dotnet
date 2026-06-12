using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Queries.Events;

public class GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<GetEventByIdQuery, EventResponse?>
{
    public async Task<EventResponse?> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await eventRepository.GetByIdAsync(query.Id);
        return entity is null ? null : mapper.Map<EventResponse>(entity);
    }
}
