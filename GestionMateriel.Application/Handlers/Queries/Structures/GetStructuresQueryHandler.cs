using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Queries.Structures;

public class GetStructuresQueryHandler(IStructureRepository structureRepository, IMapper mapper) : IRequestHandler<GetStructuresQuery, IEnumerable<StructureResponse>>
{
    public async Task<IEnumerable<StructureResponse>> Handle(GetStructuresQuery query, CancellationToken cancellationToken)
    {
        var entities = await structureRepository.GetAllAsync();
        return entities.Select(s => mapper.Map<StructureResponse>(s));
    }
}
