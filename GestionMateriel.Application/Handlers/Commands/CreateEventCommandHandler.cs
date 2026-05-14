using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public CreateEventCommandHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventResponse> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        if (request.EndDate < request.StartDate)
        {
            throw new InvalidOperationException("EndDate must be greater than or equal to StartDate.");
        }

        var entity = _mapper.Map<Event>(request);
        await _eventRepository.AddAsync(entity);
        await _eventRepository.SaveChangesAsync();

        return _mapper.Map<EventResponse>(entity);
    }
}
