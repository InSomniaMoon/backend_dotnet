using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<GetEventByIdQuery, EventResponse?>
{
    public async Task<EventResponse?> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await eventRepository.GetByIdAsync(query.Id);
        return entity is null ? null : mapper.Map<EventResponse>(entity);
    }
}
