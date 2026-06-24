using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Events;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetActualEventsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Only_Future_Events()
    {
        await using var db = TestHelper.CreateDbContext();
        var now = DateTime.UtcNow;
        db.Structures.Add(new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = Domain.Enums.StructureTypeEnum.Groupe });
        db.Events.AddRange(
            new Event
            {
                Id = 1,
                Name = "Future",
                StructureId = 1,
                StartDate = now.AddDays(1),
                EndDate = now.AddDays(5)
            },
            new Event
            {
                Id = 2,
                Name = "Actual",
                StructureId = 1,
                StartDate = now.AddDays(-5),
                EndDate = now.AddDays(1)
            }
        );
        await db.SaveChangesAsync();
        var handler = new GetActualEventsQueryHandler(db);
        var result = await handler.Handle(new GetActualEventsQuery(), CancellationToken.None);
        IEnumerable<EventResponse> eventResponses = result as EventResponse[] ?? result.ToArray();
        Assert.Single(eventResponses);
        Assert.Equal("Actual", eventResponses.First().Name);
    }
}
