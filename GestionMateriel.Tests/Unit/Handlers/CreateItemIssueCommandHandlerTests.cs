using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items.Issues;
using GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemIssueCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Issue()
    {
        await using var db = TestHelper.CreateDbContext();
        var handler = new CreateItemIssueCommandHandler(db);
        var result = await handler.Handle(new CreateItemIssueCommand(1, 1, new CreateItemIssueRequest
        {
            Usable = false,
            Value = "Broken zipper",
            AffectedQuantity = 1
        }), CancellationToken.None);
        Assert.Equal("Broken zipper", result.Value);
        Assert.Equal(1, db.ItemIssues.Count());
    }
}
