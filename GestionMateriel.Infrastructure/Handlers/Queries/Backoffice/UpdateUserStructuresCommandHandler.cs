using GestionMateriel.Application.Features.Backoffice.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Queries.Backoffice;

public class UpdateUserStructuresCommandHandler(
    GestionMaterielDbContext db
) : IRequestHandler<UpdateUserStructuresCommand, bool>
{
    public async Task<bool> Handle(UpdateUserStructuresCommand request, CancellationToken cancellationToken)
    {
        var userStructures = await db.UserStructures
            .IgnoreQueryFilters()
            .Where(us => us.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        // Remove existing structures
        db.UserStructures.RemoveRange(userStructures);

        // Add new structures
        var newUserStructures = request.Structures.Select(s => new Domain.Entities.UserStructure
        {
            UserId = request.UserId,
            StructureId = s.StructureId,
            Role = RoleEnum.FromString(s.Role),
        });

        await db.UserStructures.AddRangeAsync(newUserStructures, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}