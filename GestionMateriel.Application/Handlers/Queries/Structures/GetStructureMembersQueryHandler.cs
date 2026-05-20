using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries.Structures;

public class GetStructureMembersQueryHandler(IStructureRepository structureRepository, IMapper mapper) : IRequestHandler<GetStructureMembersQuery, IEnumerable<StructureMemberResponse>>
{
    public async Task<IEnumerable<StructureMemberResponse>> Handle(GetStructureMembersQuery query, CancellationToken cancellationToken)
    {
        var structure = await structureRepository.GetWithMembersAsync(query.StructureId);
        if (structure is null)
        {
            return [];
        }

        return structure.UserStructures.Select(us => mapper.Map<StructureMemberResponse>(us));
    }
}
