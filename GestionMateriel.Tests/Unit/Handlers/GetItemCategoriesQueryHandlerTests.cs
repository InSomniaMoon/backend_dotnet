using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemCategoriesQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Categories_By_Structure()
    {
        var repoMock = new Mock<IItemCategoryRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByStructureAsync()).ReturnsAsync(
        [
            new() { Id = 1, Name = "Cuisine", StructureId = 1 }
        ]);

        mapperMock.Setup(m => m.Map<ItemCategoryResponse>(It.IsAny<ItemCategory>()))
            .Returns<ItemCategory>(c => new ItemCategoryResponse { Id = c.Id, Name = c.Name });

        var handler = new GetItemCategoriesQueryHandler(repoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetItemCategoriesQuery(), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("Cuisine", result[0].Name);
    }
}
