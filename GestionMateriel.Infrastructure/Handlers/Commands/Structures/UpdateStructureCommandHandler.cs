using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Structures.Commands;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Structures;

public class UpdateStructureCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<UpdateStructureCommand, StructureResponse?>
{
    public async Task<StructureResponse?> Handle(UpdateStructureCommand command, CancellationToken cancellationToken)
    {
        var entity = await db.Structures.Include(s => s.UserStructures).FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken);
        if (entity is null) return null;

        entity.Color = command.Color;
        entity.Name = command.Name;

        // update Depedant UserStructures. each user has role user for the structure
        var existingMembers = entity.UserStructures.Select(us => us.UserId).ToList();
        var membersToAdd = command.MembersIds.Except(existingMembers).ToList();
        var membersToRemove = existingMembers.Except(command.MembersIds).ToList();
        foreach (var memberId in membersToAdd)
        {
            entity.UserStructures.Add(new Domain.Entities.UserStructure
            {
                UserId = memberId,
                StructureId = entity.Id,
                Role = RoleEnum.User
            });
        }
        foreach (var memberId in membersToRemove)
        {
            var userStructure = entity.UserStructures.FirstOrDefault(us => us.UserId == memberId);
            if (userStructure != null)
            {
                entity.UserStructures.Remove(userStructure);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<StructureResponse>(entity);
    }
}
