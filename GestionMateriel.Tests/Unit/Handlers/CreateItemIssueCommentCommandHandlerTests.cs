using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items.Issues;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemIssueCommentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Issue_Not_Found()
    {
        var issueRepoMock = new Mock<IItemIssueRepository>();
        var commentRepoMock = new Mock<IItemIssueCommentRepository>();
        var mapperMock = new Mock<IMapper>();

        issueRepoMock.Setup(r => r.GetByIdAsync(77)).ReturnsAsync((ItemIssue?)null);

        var handler = new CreateItemIssueCommentCommandHandler(issueRepoMock.Object, commentRepoMock.Object, mapperMock.Object);

        var result = await handler.Handle(
            new CreateItemIssueCommentCommand(77, new CreateItemIssueCommentRequest { Comment = "test", UserId = 1 }),
            CancellationToken.None);

        Assert.Null(result);
        commentRepoMock.Verify(r => r.AddAsync(It.IsAny<ItemIssueComment>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Create_Comment_When_Issue_Exists()
    {
        var issueRepoMock = new Mock<IItemIssueRepository>();
        var commentRepoMock = new Mock<IItemIssueCommentRepository>();
        var mapperMock = new Mock<IMapper>();

        issueRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(new ItemIssue { Id = 5, ItemId = 1, Value = "x", ReportedBy = 1, AffectedQuantity = 1 });

        var comment = new ItemIssueComment { Id = 3, Comment = "OK", UserId = 1 };

        mapperMock.Setup(m => m.Map<ItemIssueComment>(It.IsAny<CreateItemIssueCommentRequest>())).Returns(comment);
        mapperMock.Setup(m => m.Map<ItemIssueCommentResponse>(comment)).Returns(new ItemIssueCommentResponse { Id = 3, ItemIssueId = 5, Comment = "OK" });

        var handler = new CreateItemIssueCommentCommandHandler(issueRepoMock.Object, commentRepoMock.Object, mapperMock.Object);

        var result = await handler.Handle(
            new CreateItemIssueCommentCommand(5, new CreateItemIssueCommentRequest { Comment = "OK", UserId = 1 }),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(5, comment.ItemIssueId);
        commentRepoMock.Verify(r => r.AddAsync(comment), Times.Once);
        commentRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
