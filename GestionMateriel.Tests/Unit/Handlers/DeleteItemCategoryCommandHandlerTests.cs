using GestionMateriel.Application.Commands;
using GestionMateriel.Application.Handlers.Commands.Items.Categories;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class DeleteItemCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_False_When_Category_Not_Found()
    {
        var repoMock = new Mock<IItemCategoryRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((GestionMateriel.Domain.Entities.ItemCategory?)null);

        var handler = new DeleteItemCategoryCommandHandler(repoMock.Object);

        var result = await handler.Handle(new DeleteItemCategoryCommand(99), CancellationToken.None);

        Assert.False(result);
        repoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
