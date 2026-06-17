using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Events;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetActualEventsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Only_Future_Events()
    {
        using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Events.AddRange(
            new Event { Id = 1, Name = "Future", StructureId = 1, StartDate = now.AddDays(1), EndDate = now.AddDays(5) },
            new Event { Id = 2, Name = "Past", StructureId = 1, StartDate = now.AddDays(-5), EndDate = now.AddDays(-1) }
        );
        await db.SaveChangesAsync();
        var handler = new GetActualEventsQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetActualEventsQuery(), CancellationToken.None);
        Assert.Single(result);
        Assert.Equal("Future", result.First().Name);
    }
}
