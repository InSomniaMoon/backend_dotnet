using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Users;

public class CreateUserCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateUserCommand, UserResponse>
{
    public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var existing = await db.Users.AnyAsync(u => u.Email == command.Request.Email, cancellationToken);
        if (existing)
            throw new InvalidOperationException("An account already exists with this email.");

        var user = new User
        {
            FirstName = command.Request.FirstName,
            LastName = command.Request.LastName,
            Email = command.Request.Email,
            Phone = command.Request.Phone,
            Password = BCrypt.Net.BCrypt.HashPassword(command.Request.Password),
            Role = RoleEnum.FromString(command.Request.Role)
        };

        await db.Users.AddAsync(user, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<UserResponse>(user);
    }
}
