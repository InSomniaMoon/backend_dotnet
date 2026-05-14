using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Item_And_Return_Response()
    {
        var repoMock = new Mock<IItemRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new CreateItemRequest
        {
            Name = "Tente 2 places",
            CategoryId = 1,
            StructureId = 1,
            Stock = 3,
            Usable = true
        };

        var createdEntity = new Item
        {
            Id = 42,
            Name = request.Name,
            CategoryId = request.CategoryId,
            StructureId = request.StructureId,
            Stock = request.Stock,
            Usable = request.Usable
        };

        mapperMock
            .Setup(m => m.Map<Item>(request))
            .Returns(createdEntity);

        mapperMock
            .Setup(m => m.Map<ItemResponse>(createdEntity))
            .Returns(new ItemResponse { Id = 42, Name = request.Name });

        repoMock
            .Setup(r => r.AddAsync(createdEntity))
            .ReturnsAsync(createdEntity);

        var handler = new CreateItemCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new CreateItemCommand(request), CancellationToken.None);

        Assert.Equal(42, result.Id);
        Assert.Equal("Tente 2 places", result.Name);
        repoMock.Verify(r => r.AddAsync(createdEntity), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
