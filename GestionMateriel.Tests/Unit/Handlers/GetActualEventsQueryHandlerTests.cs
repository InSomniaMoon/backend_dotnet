using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries.Events;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetActualEventsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Actual_Events()
    {
        var eventRepoMock = new Mock<IEventRepository>();
        var mapperMock = new Mock<IMapper>();

        var events = new List<Event>
        {
            new() { Id = 3, Name = "Formation" },
            new() { Id = 4, Name = "Reunion" }
        };

        eventRepoMock
            .Setup(r => r.GetActualEventsAsync())
            .ReturnsAsync(events);

        mapperMock
            .Setup(m => m.Map<EventResponse>(It.IsAny<Event>()))
            .Returns<Event>(e => new EventResponse { Id = e.Id, Name = e.Name });

        var handler = new GetActualEventsQueryHandler(eventRepoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetActualEventsQuery(), CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Id == 3 && r.Name == "Formation");
        Assert.Contains(result, r => r.Id == 4 && r.Name == "Reunion");
        mapperMock.Verify(m => m.Map<EventResponse>(It.IsAny<Event>()), Times.Exactly(2));
    }
}
