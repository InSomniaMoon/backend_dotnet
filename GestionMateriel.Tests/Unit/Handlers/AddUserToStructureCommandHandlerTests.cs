using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Commands.Structures;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class AddUserToStructureCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Add_User_To_Structure()
    {
        using var db = TestHelper.CreateDbContext();
        db.Users.Add(new User { Id = 1, FirstName = "Alice", LastName = "D", Email = "a@b.com", Password = "x", Role = RoleEnum.User });
        db.Structures.Add(new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe });
        await db.SaveChangesAsync();
        var handler = new AddUserToStructureCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new AddUserToStructureCommand(new AddUserToStructureRequest { UserId = 1, StructureId = 1, Role = "user" }), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(1, db.UserStructures.Count());
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_User_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        db.Structures.Add(new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe });
        await db.SaveChangesAsync();
        var handler = new AddUserToStructureCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new AddUserToStructureCommand(new AddUserToStructureRequest { UserId = 99, StructureId = 1, Role = "user" }), CancellationToken.None);
        Assert.Null(result);
    }
}
