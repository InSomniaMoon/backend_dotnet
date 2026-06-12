using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Commands.Events;

public class CreateEventCommandHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<CreateEventCommand, EventResponse>
{
    public async Task<EventResponse> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        if (request.EndDate < request.StartDate)
        {
            throw new InvalidOperationException("EndDate must be greater than or equal to StartDate.");
        }

        var entity = mapper.Map<Event>(request);
        await eventRepository.AddAsync(entity);
        await eventRepository.SaveChangesAsync();

        return mapper.Map<EventResponse>(entity);
    }
}
