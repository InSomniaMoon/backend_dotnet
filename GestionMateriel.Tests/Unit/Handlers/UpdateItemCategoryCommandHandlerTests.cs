using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Categories;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class UpdateItemCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Category_Not_Found()
    {
        var repoMock = new Mock<IItemCategoryRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((ItemCategory?)null);

        var handler = new UpdateItemCategoryCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(
            new UpdateItemCategoryCommand(42, new UpdateItemCategoryRequest { Name = "Nouveau" }),
            CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Should_Update_Category_When_Found()
    {
        var repoMock = new Mock<IItemCategoryRepository>();
        var mapperMock = new Mock<IMapper>();

        var category = new ItemCategory { Id = 2, Name = "Ancien" };

        repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(category);

        mapperMock
            .Setup(m => m.Map(It.IsAny<UpdateItemCategoryRequest>(), category))
            .Callback<UpdateItemCategoryRequest, ItemCategory>((req, c) => c.Name = req.Name);

        mapperMock
            .Setup(m => m.Map<ItemCategoryResponse>(category))
            .Returns(new ItemCategoryResponse { Id = 2, Name = "Nouveau" });

        var handler = new UpdateItemCategoryCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(
            new UpdateItemCategoryCommand(2, new UpdateItemCategoryRequest { Name = "Nouveau" }),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Nouveau", category.Name);
        repoMock.Verify(r => r.UpdateAsync(category), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
