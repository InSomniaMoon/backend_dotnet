using MediatR;

namespace GestionMateriel.Application.Commands;

public record RemoveEventSubscriptionCommand(int EventId, int ItemId) : IRequest<bool>;
