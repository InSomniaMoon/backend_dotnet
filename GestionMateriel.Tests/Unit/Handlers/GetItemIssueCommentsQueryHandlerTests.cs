using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemIssueCommentsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Comments()
    {
        var repoMock = new Mock<IItemIssueCommentRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByIssueAsync(3)).ReturnsAsync(new List<ItemIssueComment>
        {
            new() { Id = 1, ItemIssueId = 3, UserId = 5, Comment = "Vu" }
        });

        mapperMock.Setup(m => m.Map<ItemIssueCommentResponse>(It.IsAny<ItemIssueComment>()))
            .Returns<ItemIssueComment>(c => new ItemIssueCommentResponse { Id = c.Id, ItemIssueId = c.ItemIssueId, Comment = c.Comment });

        var handler = new GetItemIssueCommentsQueryHandler(repoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetItemIssueCommentsQuery(3), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("Vu", result[0].Comment);
    }
}
