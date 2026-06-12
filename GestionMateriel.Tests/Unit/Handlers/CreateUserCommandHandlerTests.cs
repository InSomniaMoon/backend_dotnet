using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Users;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Commands.Users;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_User()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new CreateUserCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new CreateUserCommand(new CreateUserRequest
        {
            FirstName = "Alice",
            LastName = "D",
            Email = "alice@test.com",
            Password = "Pass123!",
            Role = "user"
        }), CancellationToken.None);
        Assert.Equal("alice@test.com", result.Email);
        Assert.Equal(1, db.Users.Count());
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Email_Taken()
    {
        using var db = TestHelper.CreateDbContext();
        db.Users.Add(new User { Id = 1, FirstName = "X", LastName = "Y", Email = "alice@test.com", Password = "x", Role = RoleEnum.User });
        await db.SaveChangesAsync();
        var handler = new CreateUserCommandHandler(db, TestHelper.CreateMapper());
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CreateUserCommand(new CreateUserRequest
            {
                FirstName = "Alice",
                LastName = "D",
                Email = "alice@test.com",
                Password = "Pass123!",
                Role = "user"
            }), CancellationToken.None));
    }
}
