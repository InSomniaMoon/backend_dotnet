using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Queries.Items.Issues;
using GestionMateriel.Tests;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetOpenItemIssuesQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Only_Open_Issues()
    {
        using var db = TestHelper.CreateDbContext();
        db.ItemIssues.AddRange(
            new ItemIssue { Id = 1, ItemId = 1, Value = "Open", Status = IssueStatusEnum.Open, ReportedBy = 1 },
            new ItemIssue { Id = 2, ItemId = 1, Value = "Resolved", Status = IssueStatusEnum.Resolved, ReportedBy = 1 }
        );
        await db.SaveChangesAsync();
        var handler = new GetOpenItemIssuesQueryHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new GetOpenItemIssuesQuery(), CancellationToken.None);
        Assert.Single(result);
    }
}
