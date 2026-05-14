using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class UpdateStructureCommandHandler : IRequestHandler<UpdateStructureCommand, StructureResponse?>
{
    private readonly IStructureRepository _structureRepository;
    private readonly IMapper _mapper;

    public UpdateStructureCommandHandler(IStructureRepository structureRepository, IMapper mapper)
    {
        _structureRepository = structureRepository;
        _mapper = mapper;
    }

    public async Task<StructureResponse?> Handle(UpdateStructureCommand command, CancellationToken cancellationToken)
    {
        var entity = await _structureRepository.GetByIdAsync(command.Id);
        if (entity is null)
        {
            return null;
        }

        _mapper.Map(command.Request, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _structureRepository.UpdateAsync(entity);
        await _structureRepository.SaveChangesAsync();

        return _mapper.Map<StructureResponse>(entity);
    }
}
