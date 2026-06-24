using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Users.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Structures;

public class GetUserStructuresQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetUserStructuresQuery, UserWithStructuresResponse?>
{
    public async Task<UserWithStructuresResponse?> Handle(GetUserStructuresQuery query,
        CancellationToken cancellationToken)
    {
        var user = await db.Users
            .Include(u => u.UserStructures)
            .ThenInclude(us => us.Structure)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);
        return user is null ? null : new UserWithStructuresResponse()
        {
            Email = user.Email,
            Structures = [.. user.UserStructures.Select(us => new StructureWithRoleResponse()
            {
                Id = us.Structure.Id,
                Name = us.Structure.Name,
                CodeStructure = us.Structure.CodeStructure,
                Type = us.Structure.Type.ToString(),
                Color = us.Structure.Color,
                Image = us.Structure.Image,
                NomStructure = us.Structure.NomStructure,
                ParentCode = us.Structure.ParentCode,
                Role = us.Role.ToString(),

            })]
        };
    }
}
