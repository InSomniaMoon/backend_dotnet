using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Structures;

public class GetStructuresQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetStructuresQuery, IEnumerable<StructureResponse>>
{
    public async Task<IEnumerable<StructureResponse>> Handle(GetStructuresQuery query,
        CancellationToken cancellationToken)
    {
        var entities = await db.Structures.AsNoTracking().ToListAsync(cancellationToken);
        return entities.Select(mapper.Map<StructureResponse>);
    }
}
