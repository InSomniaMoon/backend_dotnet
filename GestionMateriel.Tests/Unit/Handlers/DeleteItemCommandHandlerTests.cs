using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Commands.Items;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class DeleteItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_Existing_Item()
    {
        using var db = TestHelper.CreateDbContext();
        db.Items.Add(new Item { Id = 1, Name = "Tente", CategoryId = 1, StructureId = 1, Stock = 1, Usable = true });
        await db.SaveChangesAsync();

        var handler = new DeleteItemCommandHandler(db);
        var result = await handler.Handle(new DeleteItemCommand(1), CancellationToken.None);

        Assert.True(result);
        Assert.Equal(0, db.Items.Count());
    }

    [Fact]
    public async Task Handle_Should_Return_False_When_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new DeleteItemCommandHandler(db);
        var result = await handler.Handle(new DeleteItemCommand(99), CancellationToken.None);
        Assert.False(result);
    }
}
