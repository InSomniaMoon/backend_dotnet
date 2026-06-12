using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Users;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetUsersQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_Users()
    {
        using var db = TestHelper.CreateDbContext();
        db.Users.AddRange(
            new User { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com", Password = "x", Role = RoleEnum.User },
            new User { Id = 2, FirstName = "C", LastName = "D", Email = "c@d.com", Password = "x", Role = RoleEnum.User }
        );
        await db.SaveChangesAsync();
        var handler = new GetUsersQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);
        Assert.Equal(2, result.Count());
    }
}
