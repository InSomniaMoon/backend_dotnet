using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.DTOs.Responses.Structures;
using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Structures;

public class GetCurrentStructureWithChildrenQueryHandler(
    GestionMaterielDbContext db
)
    : IRequestHandler<GetCurrentStructureWithChildrenQuery,
        StructureWithChildrenResponse>
{
    public async Task<StructureWithChildrenResponse> Handle(GetCurrentStructureWithChildrenQuery request,
        CancellationToken cancellationToken)
    {
        var structure =
            await db.Structures
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.StructureId, cancellationToken);

        if (structure == null)
        {
            throw new KeyNotFoundException($"Structure with ID {request.StructureId} not found.");
        }

        var structureMask = structure.Type.ComputeCodeStructureMask(
            structure.CodeStructure);

        var children = await db.Structures
            .AsNoTracking()
            .Where(s => EF.Functions.Like(s.CodeStructure, structureMask + "%"))
            .Select(s => new StructureWithMembersResponse
            {
                Id = s.Id,
                Name = s.Name,
                CodeStructure = s.CodeStructure,
                NomStructure = s.NomStructure,
                Type = s.Type.Value,
                Members = s.UserStructures.Select(m => new UserResponse
                {
                    Id = m.User.Id,
                    FirstName = m.User.FirstName,
                    LastName = m.User.LastName,
                    Email = m.User.Email,
                    Role = m.Role.Value
                }).ToList()
            }).ToListAsync(cancellationToken);

        return new StructureWithChildrenResponse
        {
            Structure = new StructureWithMembersResponse()
            {
                Id = structure.Id,
                Name = structure.Name,
                NomStructure = structure.NomStructure,
                CodeStructure = structure.CodeStructure,
                Type = structure.Type.Value,
            },
            Children = children
        };
    }
}