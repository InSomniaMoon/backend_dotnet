using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetStructureByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Structure_When_Found()
    {
        var repoMock = new Mock<IStructureRepository>();
        var mapperMock = new Mock<IMapper>();

        var structure = new Structure { Id = 2, Name = "Groupe B", CodeStructure = "GB", NomStructure = "Groupe B", Type = GestionMateriel.Domain.Enums.StructureTypeEnum.Groupe };
        repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(structure);
        mapperMock.Setup(m => m.Map<StructureResponse>(structure)).Returns(new StructureResponse { Id = 2, Name = "Groupe B" });

        var handler = new GetStructureByIdQueryHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetStructureByIdQuery(2), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Id);
    }
}
