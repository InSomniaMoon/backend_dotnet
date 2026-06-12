using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Items.Categories;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemCategoryByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Category_When_Found()
    {
        using var db = TestHelper.CreateDbContext();
        db.ItemCategories.Add(new ItemCategory { Id = 1, Name = "Tentes", StructureId = 1 });
        await db.SaveChangesAsync();
        var handler = new GetItemCategoryByIdQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetItemCategoryByIdQuery(1), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("Tentes", result!.Name);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new GetItemCategoryByIdQueryHandler(db, TestHelper.CreateMapper());
        Assert.Null(await handler.Handle(new GetItemCategoryByIdQuery(99), CancellationToken.None));
    }
}
