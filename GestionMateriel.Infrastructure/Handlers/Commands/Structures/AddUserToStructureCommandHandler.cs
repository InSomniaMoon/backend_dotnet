using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Structures;

public class AddUserToStructureCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<AddUserToStructureCommand, StructureMemberResponse?>
{
    public async Task<StructureMemberResponse?> Handle(AddUserToStructureCommand command, CancellationToken cancellationToken)
    {
        var user = await db.Users.FindAsync([command.Request.UserId], cancellationToken);
        var structure = await db.Structures.FindAsync([command.Request.StructureId], cancellationToken);
        if (user is null || structure is null) return null;

        var existing = await db.UserStructures
            .FirstOrDefaultAsync(us => us.UserId == command.Request.UserId && us.StructureId == command.Request.StructureId, cancellationToken);

        if (existing is not null)
        {
            existing.Role = RoleEnum.FromString(command.Request.Role);
            await db.SaveChangesAsync(cancellationToken);
            return mapper.Map<StructureMemberResponse>(existing);
        }

        var userStructure = new UserStructure
        {
            UserId = command.Request.UserId,
            StructureId = command.Request.StructureId,
            Role = RoleEnum.FromString(command.Request.Role)
        };

        await db.UserStructures.AddAsync(userStructure, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<StructureMemberResponse>(userStructure);
    }
}
