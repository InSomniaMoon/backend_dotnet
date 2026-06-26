using GestionMateriel.Application.Features.Users.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Users;


public class GetUserByEmailQueryHandler(
    GestionMaterielDbContext db
) : IRequestHandler<GetUserByEmailQuery, bool>
{

    public async Task<bool> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await db.Users
            .AsNoTracking()
            .Where(u => u.Email == request.Email)
            .CountAsync(cancellationToken);


        return user > 0;
    }
}