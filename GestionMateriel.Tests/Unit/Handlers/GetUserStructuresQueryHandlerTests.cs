using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Structures;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetUserStructuresQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_User_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new GetUserStructuresQueryHandler(db, TestHelper.CreateMapper());
        Assert.Null(await handler.Handle(new GetUserStructuresQuery(99), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Return_User_With_Structures()
    {
        using var db = TestHelper.CreateDbContext();
        db.Users.Add(new User { Id = 1, FirstName = "Alice", LastName = "D", Email = "a@b.com", Password = "x", Role = RoleEnum.User });
        db.Structures.Add(new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe });
        db.UserStructures.Add(new UserStructure { UserId = 1, StructureId = 1, Role = RoleEnum.User });
        await db.SaveChangesAsync();
        var handler = new GetUserStructuresQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetUserStructuresQuery(1), CancellationToken.None);
        Assert.NotNull(result);
    }
}
