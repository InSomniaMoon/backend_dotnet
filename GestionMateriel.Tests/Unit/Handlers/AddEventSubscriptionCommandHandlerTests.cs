using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Commands.Events;

namespace GestionMateriel.Tests.Unit.Handlers;

public class AddEventSubscriptionCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Add_Subscription()
    {
        using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Structures.Add(new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe });
        db.Events.Add(new Event { Id = 1, Name = "E", StructureId = 1, StartDate = now, EndDate = now.AddDays(1) });
        db.Items.Add(new Item { Id = 10, Name = "Tente", CategoryId = 1, StructureId = 1, Stock = 5, Usable = true });
        await db.SaveChangesAsync();
        var handler = new AddEventSubscriptionCommandHandler(db);
        var result = await handler.Handle(new AddEventSubscriptionCommand(1, new AddEventSubscriptionRequest { ItemId = 10, Quantity = 2 }), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(2, result!.Quantity);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Event_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new AddEventSubscriptionCommandHandler(db);
        var result = await handler.Handle(new AddEventSubscriptionCommand(99, new AddEventSubscriptionRequest { ItemId = 1, Quantity = 1 }), CancellationToken.None);
        Assert.Null(result);
    }
}
