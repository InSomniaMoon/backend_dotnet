using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Events;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetEventSubscriptionsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Subscriptions_For_Event()
    {
        await using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Structures.Add(new Structure
            { Id = 1, Name = "GL", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe });
        db.Events.AddRange(
            new Event { Id = 1, Name = "E1", StructureId = 1, StartDate = now, EndDate = now.AddDays(1) },
            new Event { Id = 2, Name = "E2", StructureId = 1, StartDate = now, EndDate = now.AddDays(1) }
        );
        db.Items.AddRange(
            new Item { Id = 1, Name = "A", CategoryId = 1, StructureId = 1, Stock = 1, Usable = true },
            new Item { Id = 2, Name = "B", CategoryId = 1, StructureId = 1, Stock = 1, Usable = true }
        );
        db.EventSubscriptions.AddRange(
            new EventSubscription { EventId = 1, ItemId = 1, Quantity = 1 },
            new EventSubscription { EventId = 1, ItemId = 2, Quantity = 2 },
            new EventSubscription { EventId = 2, ItemId = 1, Quantity = 1 }
        );
        await db.SaveChangesAsync();
        var handler = new GetEventSubscriptionsQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetEventSubscriptionsQuery(1), CancellationToken.None);
        Assert.Equal(2, result.Count());
    }
}
