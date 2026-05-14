using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetStructuresQuery() : IRequest<IEnumerable<StructureResponse>>;
