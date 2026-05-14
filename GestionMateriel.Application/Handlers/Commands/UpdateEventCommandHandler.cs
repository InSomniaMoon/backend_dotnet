using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, EventResponse?>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventResponse?> Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {
        if (command.Request.EndDate < command.Request.StartDate)
        {
            throw new InvalidOperationException("EndDate must be greater than or equal to StartDate.");
        }

        var entity = await _eventRepository.GetByIdAsync(command.Id);
        if (entity is null)
        {
            return null;
        }

        _mapper.Map(command.Request, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _eventRepository.UpdateAsync(entity);
        await _eventRepository.SaveChangesAsync();

        return _mapper.Map<EventResponse>(entity);
    }
}
