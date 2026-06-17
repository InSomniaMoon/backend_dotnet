using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Infrastructure.Handlers.Commands.Events;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Event()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new CreateEventCommandHandler(db, TestHelper.CreateMapper());
        var start = DateTime.UtcNow.AddDays(1);
        var result = await handler.Handle(new CreateEventCommand(new CreateEventRequest
        {
            Name = "Camp été",
            StartDate = start,
            EndDate = start.AddDays(7),
            StructureId = 1
        }), CancellationToken.None);
        Assert.Equal("Camp été", result.Name);
        Assert.Equal(1, db.Events.Count());
    }

    [Fact]
    public async Task Handle_Should_Throw_When_EndDate_Before_StartDate()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new CreateEventCommandHandler(db, TestHelper.CreateMapper());
        var start = DateTime.UtcNow.AddDays(5);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CreateEventCommand(new CreateEventRequest
            {
                Name = "Bad",
                StartDate = start,
                EndDate = start.AddDays(-1),
                StructureId = 1
            }), CancellationToken.None));
    }
}
