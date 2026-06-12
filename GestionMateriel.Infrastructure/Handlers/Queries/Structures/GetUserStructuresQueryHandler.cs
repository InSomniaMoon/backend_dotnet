using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Queries;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Structures;

public class GetUserStructuresQueryHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<GetUserStructuresQuery, UserWithStructuresResponse?>
{
    public async Task<UserWithStructuresResponse?> Handle(GetUserStructuresQuery query, CancellationToken cancellationToken)
    {
        var user = await db.Users
            .Include(u => u.UserStructures)
                .ThenInclude(us => us.Structure)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);
        return user is null ? null : mapper.Map<UserWithStructuresResponse>(user);
    }
}
