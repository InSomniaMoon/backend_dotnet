using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Events;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetEventsByStructureQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Events_For_Structure()
    {
        await using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Events.AddRange(
            new Event { Id = 1, Name = "A", StructureId = 1, StartDate = now, EndDate = now.AddDays(1) },
            new Event { Id = 2, Name = "B", StructureId = 2, StartDate = now, EndDate = now.AddDays(1) }
        );
        await db.SaveChangesAsync();
        var handler = new GetEventsByStructureQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetEventsByStructureQuery(1), CancellationToken.None);
        Assert.Single(result);
    }
}
