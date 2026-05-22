using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands.Events;

public class DeleteEventCommandHandler(IEventRepository eventRepository) : IRequestHandler<DeleteEventCommand, bool>
{
    public async Task<bool> Handle(DeleteEventCommand command, CancellationToken cancellationToken)
    {
        var entity = await eventRepository.GetByIdAsync(command.Id);
        if (entity is null)
        {
            return false;
        }

        await eventRepository.DeleteAsync(command.Id);
        await eventRepository.SaveChangesAsync();
        return true;
    }
}
