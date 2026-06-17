using GestionMateriel.Application.DTOs.Requests.Events;

namespace GestionMateriel.Application.Commands;

public record UpdateEventCommand(int Id, UpdateEventRequest Request);
