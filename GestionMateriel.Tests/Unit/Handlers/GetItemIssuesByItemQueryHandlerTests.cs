using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Items.Issues;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemIssuesByItemQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Issues_For_Item()
    {
        using var db = TestHelper.CreateDbContext();
        db.ItemIssues.AddRange(
            new ItemIssue { Id = 1, ItemId = 1, Value = "A", Status = IssueStatusEnum.Open, ReportedBy = 1 },
            new ItemIssue { Id = 2, ItemId = 2, Value = "B", Status = IssueStatusEnum.Open, ReportedBy = 1 }
        );
        await db.SaveChangesAsync();
        var handler = new GetItemIssuesByItemQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetItemIssuesByItemQuery(1), CancellationToken.None);
        Assert.Single(result);
    }
}
