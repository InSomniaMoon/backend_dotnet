using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetStructureByIdQueryHandler : IRequestHandler<GetStructureByIdQuery, StructureResponse?>
{
    private readonly IStructureRepository _structureRepository;
    private readonly IMapper _mapper;

    public GetStructureByIdQueryHandler(IStructureRepository structureRepository, IMapper mapper)
    {
        _structureRepository = structureRepository;
        _mapper = mapper;
    }

    public async Task<StructureResponse?> Handle(GetStructureByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _structureRepository.GetByIdAsync(query.Id);
        return entity is null ? null : _mapper.Map<StructureResponse>(entity);
    }
}
