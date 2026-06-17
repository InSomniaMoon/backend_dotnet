using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Categories;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Commands.Items.Categories;

namespace GestionMateriel.Tests.Unit.Handlers;

public class UpdateItemCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Category()
    {
        using var db = TestHelper.CreateDbContext();
        db.ItemCategories.Add(new ItemCategory { Id = 1, Name = "Old", StructureId = 1 });
        await db.SaveChangesAsync();
        var handler = new UpdateItemCategoryCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new UpdateItemCategoryCommand(1, new UpdateItemCategoryRequest { Name = "New" }), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("New", result!.Name);
    }
}
