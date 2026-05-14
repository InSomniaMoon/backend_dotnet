using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;

    public DeleteEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<bool> Handle(DeleteEventCommand command, CancellationToken cancellationToken)
    {
        var entity = await _eventRepository.GetByIdAsync(command.Id);
        if (entity is null)
        {
            return false;
        }

        await _eventRepository.DeleteAsync(command.Id);
        await _eventRepository.SaveChangesAsync();
        return true;
    }
}
