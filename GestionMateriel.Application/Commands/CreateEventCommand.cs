using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Application.DTOs.Responses;

namespace GestionMateriel.Application.Commands;

public record CreateEventCommand(CreateEventRequest Request);
