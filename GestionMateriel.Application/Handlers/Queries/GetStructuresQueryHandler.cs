using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetStructuresQueryHandler : IRequestHandler<GetStructuresQuery, IEnumerable<StructureResponse>>
{
    private readonly IStructureRepository _structureRepository;
    private readonly IMapper _mapper;

    public GetStructuresQueryHandler(IStructureRepository structureRepository, IMapper mapper)
    {
        _structureRepository = structureRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StructureResponse>> Handle(GetStructuresQuery query, CancellationToken cancellationToken)
    {
        var entities = await _structureRepository.GetAllAsync();
        return entities.Select(s => _mapper.Map<StructureResponse>(s));
    }
}
