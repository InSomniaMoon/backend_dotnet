using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Queries;

public record GetStructureMembersQuery(int StructureId) : IRequest<IEnumerable<StructureMemberResponse>>;
