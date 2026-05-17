using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class UpdateStructureCommandHandler(IStructureRepository structureRepository, IMapper mapper) : IRequestHandler<UpdateStructureCommand, StructureResponse?>
{
    public async Task<StructureResponse?> Handle(UpdateStructureCommand command, CancellationToken cancellationToken)
    {
        var entity = await structureRepository.GetByIdAsync(command.Id);
        if (entity is null)
        {
            return null;
        }

        mapper.Map(command.Request, entity);
        await structureRepository.UpdateAsync(entity);
        await structureRepository.SaveChangesAsync();

        return mapper.Map<StructureResponse>(entity);
    }
}
