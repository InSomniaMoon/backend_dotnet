using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Structures;

public class GetStructureMembersQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetStructureMembersQuery, IEnumerable<StructureMemberResponse>>
{
    public async Task<IEnumerable<StructureMemberResponse>> Handle(GetStructureMembersQuery query, CancellationToken cancellationToken)
    {
        var structure = await db.Structures
            .Include(s => s.UserStructures)
                .ThenInclude(us => us.User)
            .FirstOrDefaultAsync(s => s.Id == query.StructureId, cancellationToken);

        if (structure is null) return [];
        return structure.UserStructures.Select(mapper.Map<StructureMemberResponse>);
    }
}
