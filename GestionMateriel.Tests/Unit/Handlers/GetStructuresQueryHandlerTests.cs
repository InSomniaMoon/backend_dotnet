using GestionMateriel.Application.Features.Structures.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Structures;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetStructuresQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_Structures()
    {
        await using var db = TestHelper.CreateDbContext();
        db.Structures.AddRange(
            new Structure { Id = 1, Name = "A", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe },
            new Structure { Id = 2, Name = "B", CodeStructure = "GL2", Type = StructureTypeEnum.Groupe }
        );
        await db.SaveChangesAsync();
        var handler = new GetStructuresQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetStructuresQuery(), CancellationToken.None);
        Assert.Equal(2, result.Count());
    }
}
