using GestionMateriel.Application.DTOs.Common;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Structures;

public class GetStructureMembersQueryHandler(GestionMaterielDbContext db)
    : IRequestHandler<GetStructureMembersQuery, PaginatedResponse<UserWithStructuresResponse>>
{
    public async Task<PaginatedResponse<UserWithStructuresResponse>> Handle(
        GetStructureMembersQuery query,
        CancellationToken cancellationToken)
    {
        query.Validate();
        var structure = await db.Structures
            .AsNoTracking()
            .Where(s => s.Id == query.StructureId)
            .Select(s => new { Code = s.CodeStructure, s.Type })
            .FirstOrDefaultAsync(cancellationToken);

        if (structure is null)
        {
            throw new KeyNotFoundException($"Structure avec l'id {query.StructureId} inexistante.");
        }

        var usersQuery = db.Users
            .AsNoTracking()
            .Include(u => u.UserStructures)
            .Where(u => u.UserStructures.Any(us => EF.Functions.Like(us.Structure.CodeStructure,
                structure!.Type.ComputeCodeStructureMask(structure.Code) + "%")))
            .Select(user => new UserWithStructuresResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Structures = user.UserStructures.Select(us => new StructureWithRoleResponse()
                {
                    Role = us.Role.Value,
                    Id = us.Structure.Id,
                    Name = us.Structure.Name,
                    Color = us.Structure.Color
                })
            });

        usersQuery = query.OrderBy switch
        {
            "firstName" when query.OrderDir == "desc" => usersQuery.OrderByDescending(u => u.FirstName),
            "firstName" => usersQuery.OrderBy(u => u.FirstName),
            "lastName" when query.OrderDir == "desc" => usersQuery.OrderByDescending(u => u.LastName),
            "lastName" => usersQuery.OrderBy(u => u.LastName),
            "email" when query.OrderDir == "desc" => usersQuery.OrderByDescending(u => u.Email),
            "email" => usersQuery.OrderBy(u => u.Email),
            _ when query.OrderDir == "desc" => usersQuery.OrderByDescending(u => u.Id),
            _ => usersQuery.OrderBy(u => u.Id)
        };

        var count = await usersQuery.CountAsync(cancellationToken);
        var users = await usersQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<UserWithStructuresResponse>
        {
            TotalCount = count,
            Page = query.PageNumber,
            Size = query.PageSize,
            Data = users
        };
    }
}
