using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Structures;

public class UpdateStructureCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<UpdateStructureCommand, StructureResponse?>
{
    public async Task<StructureResponse?> Handle(UpdateStructureCommand command, CancellationToken cancellationToken)
    {
        var entity = await db.Structures.FindAsync([command.Id], cancellationToken);
        if (entity is null) return null;

        mapper.Map(command.Request, entity);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<StructureResponse>(entity);
    }
}
