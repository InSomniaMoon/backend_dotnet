using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Events;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetEventByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Event()
    {
        await using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Events.Add(new Event { Id = 1, Name = "Camp", StructureId = 1, StartDate = now, EndDate = now.AddDays(1) });
        await db.SaveChangesAsync();
        var handler = new GetEventByIdQueryHandler(db);
        var result = await handler.Handle(new GetEventByIdQuery(1), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("Camp", result.Name);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Not_Found()
    {
        await using var db = TestHelper.CreateDbContext();
        var handler = new GetEventByIdQueryHandler(db);
        Assert.Null(await handler.Handle(new GetEventByIdQuery(99), CancellationToken.None));
    }
}
