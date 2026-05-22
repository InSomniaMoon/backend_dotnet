using GestionMateriel.Application.Commands;
using GestionMateriel.Application.Handlers.Commands.Events;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class DeleteEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_False_When_Event_Not_Found()
    {
        var eventRepoMock = new Mock<IEventRepository>();

        eventRepoMock
            .Setup(r => r.GetByIdAsync(123))
            .ReturnsAsync((GestionMateriel.Domain.Entities.Event?)null);

        var handler = new DeleteEventCommandHandler(eventRepoMock.Object);

        var result = await handler.Handle(new DeleteEventCommand(123), CancellationToken.None);

        Assert.False(result);
        eventRepoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        eventRepoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
