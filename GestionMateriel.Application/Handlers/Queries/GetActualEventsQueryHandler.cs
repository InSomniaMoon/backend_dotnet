using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetActualEventsQueryHandler : IRequestHandler<GetActualEventsQuery, IEnumerable<EventResponse>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetActualEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventResponse>> Handle(GetActualEventsQuery query, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetActualEventsAsync();
        return events.Select(e => _mapper.Map<EventResponse>(e));
    }
}
