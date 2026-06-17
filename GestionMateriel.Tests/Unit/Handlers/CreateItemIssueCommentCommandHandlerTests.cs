using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items.Issues;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemIssueCommentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Comment()
    {
        using var db = TestHelper.CreateDbContext();
        db.ItemIssues.Add(new ItemIssue { Id = 5, ItemId = 1, Value = "Issue", Status = IssueStatusEnum.Open, ReportedBy = 1 });
        await db.SaveChangesAsync();
        var handler = new CreateItemIssueCommentCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new CreateItemIssueCommentCommand(5, new CreateItemIssueCommentRequest { Comment = "Looking into it", UserId = 2 }), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(1, db.ItemIssueComments.Count());
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Issue_Not_Found()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new CreateItemIssueCommentCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new CreateItemIssueCommentCommand(99, new CreateItemIssueCommentRequest { Comment = "x", UserId = 1 }), CancellationToken.None);
        Assert.Null(result);
    }
}
