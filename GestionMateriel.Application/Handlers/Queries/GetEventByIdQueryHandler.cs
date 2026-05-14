using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventResponse?>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventResponse?> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _eventRepository.GetByIdAsync(query.Id);
        return entity is null ? null : _mapper.Map<EventResponse>(entity);
    }
}
