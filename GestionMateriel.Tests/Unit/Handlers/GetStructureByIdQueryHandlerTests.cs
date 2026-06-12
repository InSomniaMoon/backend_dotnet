using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Structures;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetStructureByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Structure_When_Found()
    {
        using var db = TestHelper.CreateDbContext();
        db.Structures.Add(new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe });
        await db.SaveChangesAsync();
        var handler = new GetStructureByIdQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetStructureByIdQuery(1), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("GL", result!.Name);
    }
}
