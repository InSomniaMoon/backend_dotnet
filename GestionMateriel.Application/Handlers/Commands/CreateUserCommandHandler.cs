using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<CreateUserCommand, UserResponse>
{
    public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var existing = await userRepository.GetByEmailAsync(command.Request.Email);
        if (existing is not null)
        {
            throw new InvalidOperationException("An account already exists with this email.");
        }

        var user = new User
        {
            FirstName = command.Request.FirstName,
            LastName = command.Request.LastName,
            Email = command.Request.Email,
            Phone = command.Request.Phone,
            Password = global::BCrypt.Net.BCrypt.HashPassword(command.Request.Password),
            Role = RoleEnum.FromString(command.Request.Role)
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return mapper.Map<UserResponse>(user);
    }
}
