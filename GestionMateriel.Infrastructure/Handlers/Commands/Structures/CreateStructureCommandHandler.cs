using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Structures;

public class CreateStructureCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateStructureCommand, StructureResponse>
{
    public async Task<StructureResponse> Handle(CreateStructureCommand command, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Structure>(command.Request);
        await db.Structures.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<StructureResponse>(entity);
    }
}
