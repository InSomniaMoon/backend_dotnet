using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.DTOs.Responses.Structures;
using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Structures;

public class GetCurrentStructureWithChildrenQueryHandler(
    GestionMaterielDbContext db
)
    : IRequestHandler<GetCurrentStructureWithChildrenQuery, StructureWithChildrenResponse>
{
    public async Task<StructureWithChildrenResponse> Handle(GetCurrentStructureWithChildrenQuery request,
        CancellationToken cancellationToken)
    {
        var structure =
            await db.Structures
                .AsNoTracking()
                .Select(i => new StructureWithMembersResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    CodeStructure = i.CodeStructure,
                    NomStructure = i.NomStructure,
                    Type = i.Type.Value,
                    Color = i.Color,
                    Image = i.Image,
                    ParentCode = i.ParentCode,
                    Members = i.UserStructures.Select(m => new UserResponse
                    {
                        Id = m.User.Id,
                        FirstName = m.User.FirstName,
                        LastName = m.User.LastName,
                        Email = m.User.Email,
                        Role = m.Role.Value
                    }).ToList()
                })
                .FirstOrDefaultAsync(s => s.Id == request.StructureId, cancellationToken) ?? throw new KeyNotFoundException($"Structure with ID {request.StructureId} not found.");
        var structureMask = StructureTypeEnum.FromString(structure.Type).ComputeCodeStructureMask(
            structure.CodeStructure);

        var children = await db.Structures
            .AsNoTracking()
            .Where(s => EF.Functions.Like(s.CodeStructure, structureMask + "%"))
            .Where(s => s.Id != structure.Id)
            .Select(s => new StructureWithMembersResponse
            {
                Id = s.Id,
                Name = s.Name,
                CodeStructure = s.CodeStructure,
                NomStructure = s.NomStructure,
                Type = s.Type.Value,
                Color = s.Color,
                Image = s.Image,
                ParentCode = s.ParentCode,
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
            Structure = structure,
            Children = children
        };
    }
}