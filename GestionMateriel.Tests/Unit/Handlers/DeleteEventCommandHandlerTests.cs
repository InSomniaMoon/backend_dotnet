using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Commands.Events;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class DeleteEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_Event()
    {
        using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Events.Add(new Event { Id = 1, Name = "E", StructureId = 1, StartDate = now, EndDate = now.AddDays(1) });
        await db.SaveChangesAsync();
        var handler = new DeleteEventCommandHandler(db);
        Assert.True(await handler.Handle(new DeleteEventCommand(1), CancellationToken.None));
        Assert.Equal(0, db.Events.Count());
    }

    [Fact]
    public async Task Handle_Should_Return_False_When_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new DeleteEventCommandHandler(db);
        Assert.False(await handler.Handle(new DeleteEventCommand(99), CancellationToken.None));
    }
}
