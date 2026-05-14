using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetEventsByStructureQuery(int StructureId) : IRequest<IEnumerable<EventResponse>>;
