using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetEventsByStructureQueryHandler : IRequestHandler<GetEventsByStructureQuery, IEnumerable<EventResponse>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetEventsByStructureQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventResponse>> Handle(GetEventsByStructureQuery query, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetEventsByStructureAsync(query.StructureId);
        return events.Select(e => _mapper.Map<EventResponse>(e));
    }
}
