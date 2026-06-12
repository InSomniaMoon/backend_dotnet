using GestionMateriel.Application.DTOs.Requests.Events;

namespace GestionMateriel.Application.Commands;

public record AddEventSubscriptionCommand(int EventId, AddEventSubscriptionRequest Request);
