using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries.Items.Issues;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetOpenItemIssuesQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Open_Issues()
    {
        var repoMock = new Mock<IItemIssueRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetOpenIssuesAsync()).ReturnsAsync(new List<ItemIssue>
        {
            new() { Id = 1, ItemId = 10, Value = "Issue 1", ReportedBy = 1, AffectedQuantity = 1 }
        });

        mapperMock.Setup(m => m.Map<ItemIssueResponse>(It.IsAny<ItemIssue>()))
            .Returns<ItemIssue>(i => new ItemIssueResponse { Id = i.Id, ItemId = i.ItemId });

        var handler = new GetOpenItemIssuesQueryHandler(repoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetOpenItemIssuesQuery(), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }
}
