using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Commands.Items.Categories;

namespace GestionMateriel.Tests.Unit.Handlers;

public class DeleteItemCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_Category()
    {
        await using var db = TestHelper.CreateDbContext();
        db.ItemCategories.Add(new ItemCategory { Id = 1, Name = "X", StructureId = 1 });
        await db.SaveChangesAsync();
        var handler = new DeleteItemCategoryCommandHandler(db);
        var result = await handler.Handle(new DeleteItemCategoryCommand(1), CancellationToken.None);
        Assert.True(result);
        Assert.Equal(0, db.ItemCategories.Count());
    }

    [Fact]
    public async Task Handle_Should_Return_False_When_Not_Found()
    {
        await using var db = TestHelper.CreateDbContext();
        var handler = new DeleteItemCategoryCommandHandler(db);
        Assert.False(await handler.Handle(new DeleteItemCategoryCommand(99), CancellationToken.None));
    }
}
