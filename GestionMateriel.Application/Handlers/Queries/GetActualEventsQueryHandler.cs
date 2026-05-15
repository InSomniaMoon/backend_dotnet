using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetActualEventsQueryHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<GetActualEventsQuery, IEnumerable<EventResponse>>
{
    public async Task<IEnumerable<EventResponse>> Handle(GetActualEventsQuery query, CancellationToken cancellationToken)
    {
        var events = await eventRepository.GetActualEventsAsync();
        return events.Select(e => mapper.Map<EventResponse>(e));
    }
}
