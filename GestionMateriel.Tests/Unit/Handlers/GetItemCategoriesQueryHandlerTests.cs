using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Items.Categories;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemCategoriesQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_Categories()
    {
        using var db = TestHelper.CreateDbContext();
        db.ItemCategories.AddRange(
            new ItemCategory { Id = 1, Name = "A", StructureId = 1 },
            new ItemCategory { Id = 2, Name = "B", StructureId = 1 }
        );
        await db.SaveChangesAsync();
        var handler = new GetItemCategoriesQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetItemCategoriesQuery(), CancellationToken.None);
        Assert.Equal(2, result.Count());
    }
}
