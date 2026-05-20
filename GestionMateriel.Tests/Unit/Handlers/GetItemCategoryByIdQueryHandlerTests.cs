using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries.Items.Categories;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetItemCategoryByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Category_Not_Found()
    {
        var repoMock = new Mock<IItemCategoryRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((ItemCategory?)null);

        var handler = new GetItemCategoryByIdQueryHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetItemCategoryByIdQuery(5), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Should_Return_Mapped_Category_When_Found()
    {
        var repoMock = new Mock<IItemCategoryRepository>();
        var mapperMock = new Mock<IMapper>();

        var category = new ItemCategory { Id = 5, Name = "Textile", StructureId = 1 };
        repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(category);
        mapperMock.Setup(m => m.Map<ItemCategoryResponse>(category)).Returns(new ItemCategoryResponse { Id = 5, Name = "Textile" });

        var handler = new GetItemCategoryByIdQueryHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetItemCategoryByIdQuery(5), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Textile", result!.Name);
    }
}
