using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Infrastructure.Handlers.Commands.Items;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Item_And_Return_Response()
    {
        using var db = TestHelper.CreateDbContext();
        var mapper = TestHelper.CreateMapper();
        var handler = new CreateItemCommandHandler(db, mapper);

        var result = await handler.Handle(new CreateItemCommand(new CreateItemRequest
        {
            Name = "Tente 2 places",
            CategoryId = 1,
            StructureId = 1,
            Stock = 3,
            Usable = true
        }), CancellationToken.None);

        Assert.Equal("Tente 2 places", result.Name);
        Assert.Equal(1, db.Items.Count());
    }
}
