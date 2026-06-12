using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Handlers.Commands.Items;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class UpdateItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Item()
    {
        using var db = TestHelper.CreateDbContext();
        db.Items.Add(new Item { Id = 1, Name = "Old", CategoryId = 1, StructureId = 1, Stock = 1, Usable = true });
        await db.SaveChangesAsync();

        var handler = new UpdateItemCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new UpdateItemCommand(1, new UpdateItemRequest { Name = "New", Stock = 5 }), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("New", result!.Name);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new UpdateItemCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new UpdateItemCommand(99, new UpdateItemRequest { Name = "X" }), CancellationToken.None);
        Assert.Null(result);
    }
}
