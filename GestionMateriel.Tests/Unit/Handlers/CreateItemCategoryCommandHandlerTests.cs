using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Category_And_Return_Response()
    {
        var repoMock = new Mock<IItemCategoryRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new CreateItemCategoryRequest { Name = "Tentes", StructureId = 1, Identified = true };
        var category = new ItemCategory { Id = 11, Name = "Tentes", StructureId = 1, Identified = true };

        mapperMock.Setup(m => m.Map<ItemCategory>(request)).Returns(category);
        mapperMock.Setup(m => m.Map<ItemCategoryResponse>(category)).Returns(new ItemCategoryResponse { Id = 11, Name = "Tentes" });

        var handler = new CreateItemCategoryCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new CreateItemCategoryCommand(request), CancellationToken.None);

        Assert.Equal(11, result.Id);
        repoMock.Verify(r => r.AddAsync(category), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
