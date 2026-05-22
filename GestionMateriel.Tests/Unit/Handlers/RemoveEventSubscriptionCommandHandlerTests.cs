using GestionMateriel.Application.Commands;
using GestionMateriel.Application.Handlers.Commands.Events;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class RemoveEventSubscriptionCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_Subscription_When_Existing()
    {
        var subscriptionRepoMock = new Mock<IEventSubscriptionRepository>();

        var existing = new EventSubscription
        {
            EventId = 5,
            ItemId = 10,
            Quantity = 2
        };

        subscriptionRepoMock
            .Setup(r => r.GetAsync(5, 10))
            .ReturnsAsync(existing);

        var handler = new RemoveEventSubscriptionCommandHandler(subscriptionRepoMock.Object);

        var result = await handler.Handle(new RemoveEventSubscriptionCommand(5, 10), CancellationToken.None);

        Assert.True(result);
        subscriptionRepoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
        subscriptionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
