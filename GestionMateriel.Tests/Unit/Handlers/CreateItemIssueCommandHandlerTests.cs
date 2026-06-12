using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items.Issues;
using GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemIssueCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Issue()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new CreateItemIssueCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new CreateItemIssueCommand(new CreateItemIssueRequest
        {
            ItemId = 1,
            Value = "Broken zipper",
            ReportedById = 1,
            AffectedQuantity = 1
        }), CancellationToken.None);
        Assert.Equal("Broken zipper", result.Value);
        Assert.Equal(1, db.ItemIssues.Count());
    }
}
