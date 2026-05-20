using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries.Events;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetEventSubscriptionsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Subscriptions()
    {
        var subscriptionRepoMock = new Mock<IEventSubscriptionRepository>();
        var mapperMock = new Mock<IMapper>();

        var subscriptions = new List<EventSubscription>
        {
            new() { EventId = 5, ItemId = 10, Quantity = 2 },
            new() { EventId = 5, ItemId = 11, Quantity = 1 }
        };

        subscriptionRepoMock
            .Setup(r => r.GetByEventAsync(5))
            .ReturnsAsync(subscriptions);

        mapperMock
            .Setup(m => m.Map<EventSubscriptionResponse>(It.IsAny<EventSubscription>()))
            .Returns<EventSubscription>(s => new EventSubscriptionResponse
            {
                EventId = s.EventId,
                ItemId = s.ItemId,
                Quantity = s.Quantity
            });

        var handler = new GetEventSubscriptionsQueryHandler(subscriptionRepoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetEventSubscriptionsQuery(5), CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.ItemId == 10 && r.Quantity == 2);
        Assert.Contains(result, r => r.ItemId == 11 && r.Quantity == 1);
        mapperMock.Verify(m => m.Map<EventSubscriptionResponse>(It.IsAny<EventSubscription>()), Times.Exactly(2));
    }
}
