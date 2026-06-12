using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Commands.Structures;

public class CreateStructureCommandHandler(IStructureRepository structureRepository, IMapper mapper) : IRequestHandler<CreateStructureCommand, StructureResponse>
{
    public async Task<StructureResponse> Handle(CreateStructureCommand command, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Structure>(command.Request);
        await structureRepository.AddAsync(entity);
        await structureRepository.SaveChangesAsync();
        return mapper.Map<StructureResponse>(entity);
    }
}
