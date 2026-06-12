using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Commands.Events;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class RemoveEventSubscriptionCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Remove_Subscription()
    {
        using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Structures.Add(new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe });
        db.Events.Add(new Event { Id = 1, Name = "E", StructureId = 1, StartDate = now, EndDate = now.AddDays(1) });
        db.Items.Add(new Item { Id = 10, Name = "Tente", CategoryId = 1, StructureId = 1, Stock = 1, Usable = true });
        db.EventSubscriptions.Add(new EventSubscription { EventId = 1, ItemId = 10, Quantity = 1 });
        await db.SaveChangesAsync();
        var handler = new RemoveEventSubscriptionCommandHandler(db);
        Assert.True(await handler.Handle(new RemoveEventSubscriptionCommand(1, 10), CancellationToken.None));
        Assert.Equal(0, db.EventSubscriptions.Count());
    }

    [Fact]
    public async Task Handle_Should_Return_False_When_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new RemoveEventSubscriptionCommandHandler(db);
        Assert.False(await handler.Handle(new RemoveEventSubscriptionCommand(1, 99), CancellationToken.None));
    }
}
