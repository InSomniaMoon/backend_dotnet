using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class UpdateStructureCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_Structure_Not_Found()
    {
        var repoMock = new Mock<IStructureRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetByIdAsync(404)).ReturnsAsync((Structure?)null);

        var handler = new UpdateStructureCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new UpdateStructureCommand(404, new UpdateStructureRequest { Name = "A", NomStructure = "A", Type = "Groupe" }), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Should_Update_Structure_When_Found()
    {
        var repoMock = new Mock<IStructureRepository>();
        var mapperMock = new Mock<IMapper>();

        var entity = new Structure { Id = 1, Name = "Ancien", NomStructure = "Ancien", CodeStructure = "X", Type = GestionMateriel.Domain.Enums.StructureTypeEnum.Unite };
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

        mapperMock
            .Setup(m => m.Map(It.IsAny<UpdateStructureRequest>(), entity))
            .Callback<UpdateStructureRequest, Structure>((req, s) => s.Name = req.Name);

        mapperMock
            .Setup(m => m.Map<StructureResponse>(entity))
            .Returns(() => new StructureResponse { Id = entity.Id, Name = entity.Name });

        var handler = new UpdateStructureCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new UpdateStructureCommand(1, new UpdateStructureRequest { Name = "Nouveau", NomStructure = "Nouveau", Type = "Groupe" }), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Nouveau", entity.Name);
        repoMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
