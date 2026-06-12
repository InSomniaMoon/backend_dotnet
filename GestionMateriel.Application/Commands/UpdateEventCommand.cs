using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Application.DTOs.Responses;

namespace GestionMateriel.Application.Commands;

public record UpdateEventCommand(int Id, UpdateEventRequest Request);
