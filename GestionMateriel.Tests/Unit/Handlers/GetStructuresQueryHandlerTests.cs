using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries.Structures;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetStructuresQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Structures()
    {
        var repoMock = new Mock<IStructureRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Structure>
        {
            new() { Id = 1, Name = "G1", NomStructure = "G1", CodeStructure = "G1", Type = GestionMateriel.Domain.Enums.StructureTypeEnum.Groupe }
        });

        mapperMock.Setup(m => m.Map<StructureResponse>(It.IsAny<Structure>()))
            .Returns<Structure>(s => new StructureResponse { Id = s.Id, Name = s.Name });

        var handler = new GetStructuresQueryHandler(repoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetStructuresQuery(), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("G1", result[0].Name);
    }
}
