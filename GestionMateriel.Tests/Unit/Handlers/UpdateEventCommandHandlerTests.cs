using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class UpdateEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Throw_When_EndDate_Is_Before_StartDate()
    {
        var repoMock = new Mock<IEventRepository>();
        var mapperMock = new Mock<IMapper>();
        var handler = new UpdateEventCommandHandler(repoMock.Object, mapperMock.Object);

        var request = new UpdateEventRequest
        {
            Name = "Camp été",
            StartDate = new DateTime(2026, 7, 10),
            EndDate = new DateTime(2026, 7, 9),
            StructureId = 1
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new UpdateEventCommand(1, request), CancellationToken.None));

        repoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Event_Not_Found()
    {
        var repoMock = new Mock<IEventRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock
            .Setup(r => r.GetByIdAsync(42))
            .ReturnsAsync((Event?)null);

        var handler = new UpdateEventCommandHandler(repoMock.Object, mapperMock.Object);

        var request = new UpdateEventRequest
        {
            Name = "Camp",
            StartDate = new DateTime(2026, 7, 10),
            EndDate = new DateTime(2026, 7, 12),
            StructureId = 1
        };

        var result = await handler.Handle(new UpdateEventCommand(42, request), CancellationToken.None);

        Assert.Null(result);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Event>()), Times.Never);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Update_Event_And_Return_Response()
    {
        var repoMock = new Mock<IEventRepository>();
        var mapperMock = new Mock<IMapper>();

        var entity = new Event
        {
            Id = 7,
            Name = "Ancien nom",
            StartDate = new DateTime(2026, 7, 1),
            EndDate = new DateTime(2026, 7, 2),
            StructureId = 1
        };

        repoMock
            .Setup(r => r.GetByIdAsync(7))
            .ReturnsAsync(entity);

        mapperMock
            .Setup(m => m.Map(It.IsAny<UpdateEventRequest>(), entity))
            .Callback<UpdateEventRequest, Event>((req, e) =>
            {
                e.Name = req.Name;
                e.StartDate = req.StartDate;
                e.EndDate = req.EndDate;
                e.StructureId = req.StructureId;
                e.Comment = req.Comment;
            });

        mapperMock
            .Setup(m => m.Map<EventResponse>(entity))
            .Returns(() => new EventResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                StructureId = entity.StructureId,
                Comment = entity.Comment
            });

        var handler = new UpdateEventCommandHandler(repoMock.Object, mapperMock.Object);

        var request = new UpdateEventRequest
        {
            Name = "Nouveau nom",
            StartDate = new DateTime(2026, 7, 10),
            EndDate = new DateTime(2026, 7, 12),
            StructureId = 2,
            Comment = "Maj"
        };

        var before = DateTime.UtcNow;
        var result = await handler.Handle(new UpdateEventCommand(7, request), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Nouveau nom", entity.Name);
        Assert.Equal(2, entity.StructureId);
        Assert.True(entity.UpdatedAt >= before);
        repoMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
