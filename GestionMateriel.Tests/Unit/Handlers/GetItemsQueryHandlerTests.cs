using AutoMapper;
using GestionMateriel.Application.Handlers.Queries.Items;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Paginated_Items()
    {
        var repoMock = new Mock<IItemRepository>();
        var mapperMock = new Mock<IMapper>();

        var items = new List<Item>
        {
            new() { Id = 1, Name = "A", CategoryId = 1, StructureId = 1 },
            new() { Id = 2, Name = "B", CategoryId = 1, StructureId = 1 },
            new() { Id = 3, Name = "C", CategoryId = 1, StructureId = 1 }
        };

        repoMock.Setup(r => r.GetByStructureAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((items, 3));
        mapperMock.Setup(m => m.Map<Application.DTOs.Responses.ItemResponse>(It.IsAny<Item>()))
            .Returns<Item>(i => new Application.DTOs.Responses.ItemResponse { Id = i.Id, Name = i.Name });

        var handler = new GetItemsQueryHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetItemsQuery(Page: 2, Size: 2), CancellationToken.None);

        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Page);
        Assert.Single(result.Data);
        Assert.Equal(3, result.Data.First().Id);
    }
}
