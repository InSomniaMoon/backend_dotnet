using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateStructureCommandHandler : IRequestHandler<CreateStructureCommand, StructureResponse>
{
    private readonly IStructureRepository _structureRepository;
    private readonly IMapper _mapper;

    public CreateStructureCommandHandler(IStructureRepository structureRepository, IMapper mapper)
    {
        _structureRepository = structureRepository;
        _mapper = mapper;
    }

    public async Task<StructureResponse> Handle(CreateStructureCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Structure>(command.Request);
        await _structureRepository.AddAsync(entity);
        await _structureRepository.SaveChangesAsync();
        return _mapper.Map<StructureResponse>(entity);
    }
}
