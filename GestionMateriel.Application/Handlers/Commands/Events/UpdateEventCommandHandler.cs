using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Commands.Events;

public class UpdateEventCommandHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<UpdateEventCommand, EventResponse?>
{
    public async Task<EventResponse?> Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {
        if (command.Request.EndDate < command.Request.StartDate)
        {
            throw new InvalidOperationException("EndDate must be greater than or equal to StartDate.");
        }

        var entity = await eventRepository.GetByIdAsync(command.Id);
        if (entity is null)
        {
            return null;
        }

        mapper.Map(command.Request, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await eventRepository.UpdateAsync(entity);
        await eventRepository.SaveChangesAsync();

        return mapper.Map<EventResponse>(entity);
    }
}
