using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands.Events;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class AddEventSubscriptionCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Event_Not_Found()
    {
        var eventRepoMock = new Mock<IEventRepository>();
        var itemRepoMock = new Mock<IItemRepository>();
        var subscriptionRepoMock = new Mock<IEventSubscriptionRepository>();
        var mapperMock = new Mock<IMapper>();

        eventRepoMock
            .Setup(r => r.GetByIdAsync(5))
            .ReturnsAsync((Event?)null);

        var handler = new AddEventSubscriptionCommandHandler(
            eventRepoMock.Object,
            itemRepoMock.Object,
            subscriptionRepoMock.Object,
            mapperMock.Object);

        var result = await handler.Handle(
            new AddEventSubscriptionCommand(5, new AddEventSubscriptionRequest { ItemId = 10, Quantity = 2 }),
            CancellationToken.None);

        Assert.Null(result);
        itemRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        subscriptionRepoMock.Verify(r => r.AddAsync(It.IsAny<EventSubscription>()), Times.Never);
        subscriptionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Update_Existing_Subscription_Quantity()
    {
        var eventRepoMock = new Mock<IEventRepository>();
        var itemRepoMock = new Mock<IItemRepository>();
        var subscriptionRepoMock = new Mock<IEventSubscriptionRepository>();
        var mapperMock = new Mock<IMapper>();

        var existing = new EventSubscription
        {
            EventId = 5,
            ItemId = 10,
            Quantity = 1
        };

        eventRepoMock
            .Setup(r => r.GetByIdAsync(5))
            .ReturnsAsync(new Event { Id = 5, Name = "Camp" });

        itemRepoMock
            .Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(new Item { Id = 10, Name = "Tente" });

        subscriptionRepoMock
            .Setup(r => r.GetAsync(5, 10))
            .ReturnsAsync(existing);

        mapperMock
            .Setup(m => m.Map<EventSubscriptionResponse>(existing))
            .Returns(new EventSubscriptionResponse { EventId = 5, ItemId = 10, Quantity = 4 });

        var handler = new AddEventSubscriptionCommandHandler(
            eventRepoMock.Object,
            itemRepoMock.Object,
            subscriptionRepoMock.Object,
            mapperMock.Object);

        var result = await handler.Handle(
            new AddEventSubscriptionCommand(5, new AddEventSubscriptionRequest { ItemId = 10, Quantity = 4 }),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(4, existing.Quantity);
        Assert.Equal(4, result!.Quantity);
        subscriptionRepoMock.Verify(r => r.AddAsync(It.IsAny<EventSubscription>()), Times.Never);
        subscriptionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Create_Subscription_When_Not_Existing()
    {
        var eventRepoMock = new Mock<IEventRepository>();
        var itemRepoMock = new Mock<IItemRepository>();
        var subscriptionRepoMock = new Mock<IEventSubscriptionRepository>();
        var mapperMock = new Mock<IMapper>();

        eventRepoMock
            .Setup(r => r.GetByIdAsync(5))
            .ReturnsAsync(new Event { Id = 5, Name = "Camp" });

        itemRepoMock
            .Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(new Item { Id = 10, Name = "Tente" });

        subscriptionRepoMock
            .Setup(r => r.GetAsync(5, 10))
            .ReturnsAsync((EventSubscription?)null);

        mapperMock
            .Setup(m => m.Map<EventSubscriptionResponse>(It.IsAny<EventSubscription>()))
            .Returns<EventSubscription>(s => new EventSubscriptionResponse
            {
                EventId = s.EventId,
                ItemId = s.ItemId,
                Quantity = s.Quantity
            });

        var handler = new AddEventSubscriptionCommandHandler(
            eventRepoMock.Object,
            itemRepoMock.Object,
            subscriptionRepoMock.Object,
            mapperMock.Object);

        var result = await handler.Handle(
            new AddEventSubscriptionCommand(5, new AddEventSubscriptionRequest { ItemId = 10, Quantity = 3 }),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(5, result!.EventId);
        Assert.Equal(10, result.ItemId);
        Assert.Equal(3, result.Quantity);
        subscriptionRepoMock.Verify(
            r => r.AddAsync(It.Is<EventSubscription>(s => s.EventId == 5 && s.ItemId == 10 && s.Quantity == 3)),
            Times.Once);
        subscriptionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
