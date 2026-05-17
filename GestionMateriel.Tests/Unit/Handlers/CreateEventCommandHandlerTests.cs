using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Events;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Event_And_Return_Response()
    {
        var eventRepoMock = new Mock<IEventRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new CreateEventRequest
        {
            Name = "Camp d'ete",
            StartDate = new DateTime(2026, 7, 10),
            EndDate = new DateTime(2026, 7, 15),
            StructureId = 1,
            CreatedById = 2,
            Comment = "Preparation logistique"
        };

        var eventEntity = new Event
        {
            Id = 12,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            StructureId = request.StructureId,
            UserId = request.CreatedById,
            Comment = request.Comment
        };

        mapperMock
            .Setup(m => m.Map<Event>(request))
            .Returns(eventEntity);

        mapperMock
            .Setup(m => m.Map<EventResponse>(eventEntity))
            .Returns(new EventResponse { Id = 12, Name = request.Name });

        eventRepoMock
            .Setup(r => r.AddAsync(eventEntity))
            .ReturnsAsync(eventEntity);

        var handler = new CreateEventCommandHandler(eventRepoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new CreateEventCommand(request), CancellationToken.None);

        Assert.Equal(12, result.Id);
        Assert.Equal("Camp d'ete", result.Name);
        eventRepoMock.Verify(r => r.AddAsync(eventEntity), Times.Once);
        eventRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
