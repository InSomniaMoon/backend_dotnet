using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Commands;

public record UpdateEventCommand(int Id, UpdateEventRequest Request) : IRequest<EventResponse?>;
