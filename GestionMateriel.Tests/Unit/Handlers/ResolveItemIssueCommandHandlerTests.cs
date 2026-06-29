using GestionMateriel.Application.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

namespace GestionMateriel.Tests.Unit.Handlers;

public class ResolveItemIssueCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Resolve_Issue()
    {
        using var db = TestHelper.CreateDbContext();
        db.ItemIssues.Add(new ItemIssue { Id = 1, ItemId = 1, Value = "Issue", Status = IssueStatusEnum.Open, ReportedBy = 1 });
        await db.SaveChangesAsync();
        var handler = new ResolveItemIssueCommandHandler(db);
        var result = await handler.Handle(new ResolveItemIssueCommand(1), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(IssueStatusEnum.Resolved.ToString(), result!.Status);
    }
}
