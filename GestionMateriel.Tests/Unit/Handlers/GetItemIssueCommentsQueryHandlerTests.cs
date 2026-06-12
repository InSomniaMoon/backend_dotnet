using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Items.Issues;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemIssueCommentsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Comments_For_Issue()
    {
        using var db = TestHelper.CreateDbContext();
        db.ItemIssues.Add(new ItemIssue { Id = 1, ItemId = 1, Value = "Issue", Status = IssueStatusEnum.Open, ReportedBy = 1 });
        db.ItemIssueComments.AddRange(
            new ItemIssueComment { Id = 1, ItemIssueId = 1, Comment = "Cmnt1", UserId = 1 },
            new ItemIssueComment { Id = 2, ItemIssueId = 1, Comment = "Cmnt2", UserId = 1 }
        );
        await db.SaveChangesAsync();
        var handler = new GetItemIssueCommentsQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetItemIssueCommentsQuery(1), CancellationToken.None);
        Assert.Equal(2, result.Count());
    }
}
