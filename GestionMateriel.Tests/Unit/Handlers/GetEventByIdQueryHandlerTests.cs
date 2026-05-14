using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetEventByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Event_Not_Found()
    {
        var eventRepoMock = new Mock<IEventRepository>();
        var mapperMock = new Mock<IMapper>();

        eventRepoMock
            .Setup(r => r.GetByIdAsync(404))
            .ReturnsAsync((Event?)null);

        var handler = new GetEventByIdQueryHandler(eventRepoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetEventByIdQuery(404), CancellationToken.None);

        Assert.Null(result);
        mapperMock.Verify(m => m.Map<EventResponse>(It.IsAny<Event>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_Mapped_Event_When_Found()
    {
        var eventRepoMock = new Mock<IEventRepository>();
        var mapperMock = new Mock<IMapper>();

        var entity = new Event { Id = 9, Name = "Weekend" };

        eventRepoMock
            .Setup(r => r.GetByIdAsync(9))
            .ReturnsAsync(entity);

        mapperMock
            .Setup(m => m.Map<EventResponse>(entity))
            .Returns(new EventResponse { Id = 9, Name = "Weekend" });

        var handler = new GetEventByIdQueryHandler(eventRepoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetEventByIdQuery(9), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(9, result!.Id);
        Assert.Equal("Weekend", result.Name);
    }
}
