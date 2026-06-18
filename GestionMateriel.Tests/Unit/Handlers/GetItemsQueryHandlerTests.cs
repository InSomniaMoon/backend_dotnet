using GestionMateriel.Application.Features.Items.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Items;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Paginated_Items()
    {
        await using var db = TestHelper.CreateDbContext();
        db.ItemCategories.Add(new ItemCategory { Id = 1, Name = "Cat", StructureId = 1 });
        db.Items.AddRange(
            new Item { Id = 1, Name = "A", CategoryId = 1, StructureId = 1, Stock = 1, Usable = true },
            new Item { Id = 2, Name = "B", CategoryId = 1, StructureId = 1, Stock = 1, Usable = true },
            new Item { Id = 3, Name = "C", CategoryId = 1, StructureId = 1, Stock = 1, Usable = true }
        );
        await db.SaveChangesAsync();
        var handler = new GetItemsQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetItemsQuery(Page: 1, Size: 2), CancellationToken.None);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Data.Count());
    }
}
