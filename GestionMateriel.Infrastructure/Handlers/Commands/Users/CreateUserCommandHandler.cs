using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Services;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Users;

public class CreateUserCommandHandler(
    GestionMaterielDbContext db,
    IPasswordResetService passwordResetService
    )
    : IRequestHandler<CreateUserCommand, UserResponse>
{
    public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var existing = await db.Users.AnyAsync(u => u.Email == command.Request.Email, cancellationToken);
        if (existing)
            throw new InvalidOperationException("Un utilisateur existe déjà avec cet email.");

        var user = new User
        {
            FirstName = command.Request.FirstName,
            LastName = command.Request.LastName,
            Email = command.Request.Email,
            Phone = command.Request.Phone,
            // random string of 12 characters
            Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString("N").Substring(0, 12)),
            Role = RoleEnum.FromString(command.Request.Role),
            UserStructures =
            [
                new UserStructure
                {
                    StructureId = command.Request.StructureId
                }
            ]
        };

        await db.Users.AddAsync(user, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        await passwordResetService.SendResetLinkAsync(user.Email, cancellationToken);
        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.Value
        };
    }
}
