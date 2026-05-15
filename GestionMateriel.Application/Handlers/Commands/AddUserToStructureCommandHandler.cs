using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class AddUserToStructureCommandHandler(
    IUserRepository userRepository,
    IStructureRepository structureRepository,
    IUserStructureRepository userStructureRepository,
    IMapper mapper) : IRequestHandler<AddUserToStructureCommand, StructureMemberResponse?>
{
    public async Task<StructureMemberResponse?> Handle(AddUserToStructureCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Request.UserId);
        var structure = await structureRepository.GetByIdAsync(command.Request.StructureId);
        if (user is null || structure is null)
        {
            return null;
        }

        var existing = await userStructureRepository.GetAsync(command.Request.UserId, command.Request.StructureId);
        if (existing is not null)
        {
            existing.Role = RoleEnum.FromString(command.Request.Role);
            await userStructureRepository.SaveChangesAsync();
            return mapper.Map<StructureMemberResponse>(existing);
        }

        var userStructure = new UserStructure
        {
            UserId = command.Request.UserId,
            StructureId = command.Request.StructureId,
            Role = RoleEnum.FromString(command.Request.Role)
        };

        await userStructureRepository.AddAsync(userStructure);
        await userStructureRepository.SaveChangesAsync();

        return mapper.Map<StructureMemberResponse>(userStructure);
    }
}
