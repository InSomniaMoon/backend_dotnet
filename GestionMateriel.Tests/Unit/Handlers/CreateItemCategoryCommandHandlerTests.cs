using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Categories;
using GestionMateriel.Infrastructure.Handlers.Commands.Items.Categories;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Category()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new CreateItemCategoryCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new CreateItemCategoryCommand(new CreateItemCategoryRequest { Name = "Tentes" }), CancellationToken.None);
        Assert.Equal("Tentes", result.Name);
        Assert.Equal(1, db.ItemCategories.Count());
    }
}
