using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Structures;

public class GetStructureByIdQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetStructureByIdQuery, StructureResponse?>
{
    public async Task<StructureResponse?> Handle(GetStructureByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await db.Structures.AsNoTracking().FirstOrDefaultAsync(s => s.Id == query.Id, cancellationToken);
        return entity is null ? null : mapper.Map<StructureResponse>(entity);
    }
}
