using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Item_Not_Found()
    {
        var repoMock = new Mock<IItemRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByIdAsync(101)).ReturnsAsync((Item?)null);

        var handler = new GetItemByIdQueryHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetItemByIdQuery(101), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Should_Return_Mapped_Item_When_Found()
    {
        var repoMock = new Mock<IItemRepository>();
        var mapperMock = new Mock<IMapper>();

        var item = new Item { Id = 3, Name = "Marmite", CategoryId = 1, StructureId = 1 };

        repoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(item);
        mapperMock.Setup(m => m.Map<ItemResponse>(item)).Returns(new ItemResponse { Id = 3, Name = "Marmite" });

        var handler = new GetItemByIdQueryHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetItemByIdQuery(3), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Marmite", result!.Name);
    }
}
