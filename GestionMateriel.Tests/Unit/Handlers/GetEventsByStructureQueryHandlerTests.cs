using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetEventsByStructureQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Events_For_Structure()
    {
        var eventRepoMock = new Mock<IEventRepository>();
        var mapperMock = new Mock<IMapper>();

        var events = new List<Event>
        {
            new() { Id = 1, Name = "Camp" },
            new() { Id = 2, Name = "Rando" }
        };

        eventRepoMock
            .Setup(r => r.GetEventsByStructureAsync(7))
            .ReturnsAsync(events);

        mapperMock
            .Setup(m => m.Map<EventResponse>(It.IsAny<Event>()))
            .Returns<Event>(e => new EventResponse { Id = e.Id, Name = e.Name });

        var handler = new GetEventsByStructureQueryHandler(eventRepoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetEventsByStructureQuery(7), CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Id == 1 && r.Name == "Camp");
        Assert.Contains(result, r => r.Id == 2 && r.Name == "Rando");
        mapperMock.Verify(m => m.Map<EventResponse>(It.IsAny<Event>()), Times.Exactly(2));
    }
}
