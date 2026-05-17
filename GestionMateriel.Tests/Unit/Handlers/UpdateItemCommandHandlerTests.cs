using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class UpdateItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Item_Not_Found()
    {
        var repoMock = new Mock<IItemRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByIdAsync(77)).ReturnsAsync((Item?)null);

        var handler = new UpdateItemCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new UpdateItemCommand(77, new UpdateItemRequest { Name = "x", CategoryId = 1, StructureId = 1 }), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Should_Update_Item_When_Found()
    {
        var repoMock = new Mock<IItemRepository>();
        var mapperMock = new Mock<IMapper>();

        var item = new Item { Id = 9, Name = "Ancien", CategoryId = 1, StructureId = 1 };
        repoMock.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(item);

        mapperMock
            .Setup(m => m.Map(It.IsAny<UpdateItemRequest>(), item))
            .Callback<UpdateItemRequest, Item>((req, i) => i.Name = req.Name);

        mapperMock
            .Setup(m => m.Map<ItemResponse>(item))
            .Returns(() => new ItemResponse { Id = item.Id, Name = item.Name });

        var handler = new UpdateItemCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new UpdateItemCommand(9, new UpdateItemRequest { Name = "Nouveau", CategoryId = 2, StructureId = 3 }), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Nouveau", item.Name);
        repoMock.Verify(r => r.UpdateAsync(item), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
