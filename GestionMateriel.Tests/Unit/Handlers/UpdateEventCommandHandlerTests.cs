using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Commands.Events;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class UpdateEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Event()
    {
        using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Events.Add(new Event { Id = 1, Name = "Old", StructureId = 1, StartDate = now, EndDate = now.AddDays(3) });
        await db.SaveChangesAsync();
        var handler = new UpdateEventCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new UpdateEventCommand(1, new UpdateEventRequest { Name = "New", StartDate = now, EndDate = now.AddDays(5) }), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("New", result!.Name);
    }
}
