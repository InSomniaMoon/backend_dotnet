using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Queries.Items;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Item_When_Found()
    {
        await using var db = TestHelper.CreateDbContext();
        db.Items.Add(new Item { Id = 1, Name = "Tente", CategoryId = 1, StructureId = 1, Stock = 2, Usable = true });
        await db.SaveChangesAsync();

        var handler = new GetItemByIdQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetItemByIdQuery(1), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Tente", result!.Name);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Not_Found()
    {
        await using var db = TestHelper.CreateDbContext();
        var handler = new GetItemByIdQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetItemByIdQuery(99), CancellationToken.None);
        Assert.Null(result);
    }
}
