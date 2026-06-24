using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Items;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Item_When_Found()
    {
        await using var db = TestHelper.CreateDbContext();
        db.Structures.Add(new Structure { Id = 1, Name = "GL", CodeStructure = "GL1", Type = Domain.Enums.StructureTypeEnum.Groupe });
        db.ItemCategories.Add(new ItemCategory { Id = 1, Name = "Tentes", StructureId = 1 });
        db.Items.Add(new Item { Id = 1, Name = "Tente", CategoryId = 1, StructureId = 1, Stock = 2, Usable = true });
        await db.SaveChangesAsync();

        var handler = new GetItemByIdQueryHandler(db);
        var result = await handler.Handle(new GetItemByIdQuery(1), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Tente", result.Name);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Not_Found()
    {
        await using var db = TestHelper.CreateDbContext();
        var handler = new GetItemByIdQueryHandler(db);
        var result = await handler.Handle(new GetItemByIdQuery(99), CancellationToken.None);
        Assert.Null(result);
    }
}
