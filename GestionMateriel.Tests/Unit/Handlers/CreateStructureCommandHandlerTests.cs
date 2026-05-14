using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateStructureCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Structure_And_Return_Response()
    {
        var repoMock = new Mock<IStructureRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new CreateStructureRequest
        {
            Name = "Groupe A",
            NomStructure = "Groupe A",
            CodeStructure = "GA",
            Type = "Groupe"
        };

        var structure = new Structure { Id = 4, Name = request.Name, NomStructure = request.NomStructure, CodeStructure = request.CodeStructure, Type = GestionMateriel.Domain.Enums.StructureTypeEnum.Groupe };

        mapperMock.Setup(m => m.Map<Structure>(request)).Returns(structure);
        mapperMock.Setup(m => m.Map<StructureResponse>(structure)).Returns(new StructureResponse { Id = 4, Name = request.Name });

        var handler = new CreateStructureCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new CreateStructureCommand(request), CancellationToken.None);

        Assert.Equal(4, result.Id);
        repoMock.Verify(r => r.AddAsync(structure), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
