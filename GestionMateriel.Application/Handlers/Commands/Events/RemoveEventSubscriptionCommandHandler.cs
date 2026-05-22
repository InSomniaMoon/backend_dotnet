using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands.Events;

public class RemoveEventSubscriptionCommandHandler(IEventSubscriptionRepository eventSubscriptionRepository) : IRequestHandler<RemoveEventSubscriptionCommand, bool>
{
    public async Task<bool> Handle(RemoveEventSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var existing = await eventSubscriptionRepository.GetAsync(command.EventId, command.ItemId);
        if (existing is null)
        {
            return false;
        }

        await eventSubscriptionRepository.DeleteAsync(existing);
        await eventSubscriptionRepository.SaveChangesAsync();
        return true;
    }
}
