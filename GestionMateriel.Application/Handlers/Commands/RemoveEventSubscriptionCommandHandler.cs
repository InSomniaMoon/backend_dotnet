using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class RemoveEventSubscriptionCommandHandler : IRequestHandler<RemoveEventSubscriptionCommand, bool>
{
    private readonly IEventSubscriptionRepository _eventSubscriptionRepository;

    public RemoveEventSubscriptionCommandHandler(IEventSubscriptionRepository eventSubscriptionRepository)
    {
        _eventSubscriptionRepository = eventSubscriptionRepository;
    }

    public async Task<bool> Handle(RemoveEventSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var existing = await _eventSubscriptionRepository.GetAsync(command.EventId, command.ItemId);
        if (existing is null)
        {
            return false;
        }

        await _eventSubscriptionRepository.DeleteAsync(existing);
        await _eventSubscriptionRepository.SaveChangesAsync();
        return true;
    }
}
