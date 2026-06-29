using GestionMateriel.Application.Features.Events.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Events;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetEventsByStructureQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Events_For_Structure()
    {
        using var db = TestHelper.CreateDbContextWithTenantFilters(structureMask: "406981800");
        db.Structures.Add(new Structure { Id = 1, Name = "GL1", CodeStructure = "406981800", Type = Domain.Enums.StructureTypeEnum.Groupe });
        db.Structures.Add(new Structure { Id = 2, Name = "GL2", CodeStructure = "406981900", Type = Domain.Enums.StructureTypeEnum.Groupe });

        var now = DateTime.UtcNow;
        db.Events.AddRange(
            new Event { Id = 1, Name = "A", StructureId = 1, CodeStructure = "406981800", StartDate = now, EndDate = now.AddDays(1) },
            new Event { Id = 2, Name = "B", StructureId = 2, CodeStructure = "406981900", StartDate = now, EndDate = now.AddDays(1) }
        );
        await db.SaveChangesAsync();
        var handler = new GetEventsByStructureQueryHandler(db);
        var result = await handler.Handle(new GetEventsByStructureQuery(now.AddDays(-1), now.AddDays(2), Domain.Enums.StructureTypeEnum.Groupe, "406981800"), CancellationToken.None);
        Assert.Single(result);
    }
}
