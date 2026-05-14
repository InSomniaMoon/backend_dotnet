using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetStructureMembersQueryHandler : IRequestHandler<GetStructureMembersQuery, IEnumerable<StructureMemberResponse>>
{
    private readonly IStructureRepository _structureRepository;
    private readonly IMapper _mapper;

    public GetStructureMembersQueryHandler(IStructureRepository structureRepository, IMapper mapper)
    {
        _structureRepository = structureRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StructureMemberResponse>> Handle(GetStructureMembersQuery query, CancellationToken cancellationToken)
    {
        var structure = await _structureRepository.GetWithMembersAsync(query.StructureId);
        if (structure is null)
        {
            return [];
        }

        return structure.UserStructures.Select(us => _mapper.Map<StructureMemberResponse>(us));
    }
}
