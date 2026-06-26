using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Users.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Backoffice;


public class GetUserStructuresHandler(
    GestionMaterielDbContext db
) : IRequestHandler<GetUserStructuresQuery, List<StructureWithRoleResponse>>
{
    public async Task<List<StructureWithRoleResponse>> Handle(GetUserStructuresQuery request, CancellationToken cancellationToken)
    {
        return await db.UserStructures.AsNoTracking()
        .IgnoreQueryFilters()
        .Where(us => us.UserId == request.UserId)
        .Include(us => us.Structure).Select(us => new StructureWithRoleResponse
        {
            Id = us.Structure.Id,
            Name = us.Structure.Name,
            CodeStructure = us.Structure.CodeStructure,
            Color = us.Structure.Color,
            NomStructure = us.Structure.NomStructure,
            Type = us.Structure.Type.ToString(),
            Role = us.Role.ToString(),
        })
        .ToListAsync(cancellationToken);


    }
}