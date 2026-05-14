using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetStructureByIdQuery(int Id) : IRequest<StructureResponse?>;
