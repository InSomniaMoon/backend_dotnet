using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Queries;

public class GetStructureByIdQueryHandler(IStructureRepository structureRepository, IMapper mapper) : IRequestHandler<GetStructureByIdQuery, StructureResponse?>
{
    public async Task<StructureResponse?> Handle(GetStructureByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await structureRepository.GetByIdAsync(query.Id);
        return entity is null ? null : mapper.Map<StructureResponse>(entity);
    }
}
