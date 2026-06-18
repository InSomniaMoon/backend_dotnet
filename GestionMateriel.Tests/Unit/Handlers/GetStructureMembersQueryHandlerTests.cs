using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Structures;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetStructureMembersQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Empty_When_Structure_Not_Found()
    {
        await using var db = TestHelper.CreateDbContext();
        var handler = new GetStructureMembersQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetStructureMembersQuery(99), CancellationToken.None);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_Should_Return_Members()
    {
        await using var db = TestHelper.CreateDbContext();
        var user = new User
            { Id = 1, FirstName = "Alice", LastName = "D", Email = "a@b.com", Password = "x", Role = RoleEnum.User };
        var structure = new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe };
        db.Users.Add(user);
        db.Structures.Add(structure);
        db.UserStructures.Add(new UserStructure { UserId = 1, StructureId = 1, Role = RoleEnum.User });
        await db.SaveChangesAsync();
        var handler = new GetStructureMembersQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetStructureMembersQuery(1), CancellationToken.None);
        Assert.Single(result);
    }
}
