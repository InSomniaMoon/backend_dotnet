using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemIssuesByItemQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Issues_For_Item()
    {
        var repoMock = new Mock<IItemIssueRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByItemAsync(6)).ReturnsAsync(new List<ItemIssue>
        {
            new() { Id = 2, ItemId = 6, Value = "Issue", ReportedBy = 1, AffectedQuantity = 1 }
        });

        mapperMock.Setup(m => m.Map<ItemIssueResponse>(It.IsAny<ItemIssue>()))
            .Returns<ItemIssue>(i => new ItemIssueResponse { Id = i.Id, ItemId = i.ItemId });

        var handler = new GetItemIssuesByItemQueryHandler(repoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetItemIssuesByItemQuery(6), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal(6, result[0].ItemId);
    }
}
