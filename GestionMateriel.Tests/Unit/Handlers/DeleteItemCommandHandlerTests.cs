using GestionMateriel.Application.Commands;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class DeleteItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_False_When_Item_Not_Found()
    {
        var repoMock = new Mock<IItemRepository>();

        repoMock.Setup(r => r.GetByIdAsync(321)).ReturnsAsync((GestionMateriel.Domain.Entities.Item?)null);

        var handler = new DeleteItemCommandHandler(repoMock.Object);

        var result = await handler.Handle(new DeleteItemCommand(321), CancellationToken.None);

        Assert.False(result);
        repoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
