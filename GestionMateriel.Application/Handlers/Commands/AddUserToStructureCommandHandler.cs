using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class AddUserToStructureCommandHandler : IRequestHandler<AddUserToStructureCommand, StructureMemberResponse?>
{
    private readonly IUserRepository _userRepository;
    private readonly IStructureRepository _structureRepository;
    private readonly IUserStructureRepository _userStructureRepository;
    private readonly IMapper _mapper;

    public AddUserToStructureCommandHandler(
        IUserRepository userRepository,
        IStructureRepository structureRepository,
        IUserStructureRepository userStructureRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _structureRepository = structureRepository;
        _userStructureRepository = userStructureRepository;
        _mapper = mapper;
    }

    public async Task<StructureMemberResponse?> Handle(AddUserToStructureCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Request.UserId);
        var structure = await _structureRepository.GetByIdAsync(command.Request.StructureId);
        if (user is null || structure is null)
        {
            return null;
        }

        var existing = await _userStructureRepository.GetAsync(command.Request.UserId, command.Request.StructureId);
        if (existing is not null)
        {
            existing.Role = Enum.Parse<RoleEnum>(command.Request.Role, true);
            await _userStructureRepository.SaveChangesAsync();
            return _mapper.Map<StructureMemberResponse>(existing);
        }

        var userStructure = new UserStructure
        {
            UserId = command.Request.UserId,
            StructureId = command.Request.StructureId,
            Role = Enum.Parse<RoleEnum>(command.Request.Role, true)
        };

        await _userStructureRepository.AddAsync(userStructure);
        await _userStructureRepository.SaveChangesAsync();

        return _mapper.Map<StructureMemberResponse>(userStructure);
    }
}
