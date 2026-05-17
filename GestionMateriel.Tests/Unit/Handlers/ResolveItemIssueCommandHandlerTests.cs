using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class ResolveItemIssueCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Mark_Issue_As_Resolved()
    {
        var issueRepoMock = new Mock<IItemIssueRepository>();
        var mapperMock = new Mock<IMapper>();

        var issue = new ItemIssue
        {
            Id = 7,
            ItemId = 1,
            Status = IssueStatusEnum.Open,
            Value = "Fermeture cassée",
            ReportedBy = 2,
            AffectedQuantity = 1
        };

        issueRepoMock
            .Setup(r => r.GetByIdAsync(7))
            .ReturnsAsync(issue);

        mapperMock
            .Setup(m => m.Map<ItemIssueResponse>(issue))
            .Returns(new ItemIssueResponse { Id = issue.Id, Status = IssueStatusEnum.Resolved.ToString() });

        var handler = new ResolveItemIssueCommandHandler(issueRepoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new ResolveItemIssueCommand(7), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(IssueStatusEnum.Resolved, issue.Status);
        Assert.NotNull(issue.ResolutionDate);
        issueRepoMock.Verify(r => r.UpdateAsync(issue), Times.Once);
        issueRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
