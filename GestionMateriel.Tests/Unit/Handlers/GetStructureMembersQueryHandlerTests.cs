using AutoMapper;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetStructureMembersQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Empty_When_Structure_Not_Found()
    {
        var structureRepoMock = new Mock<IStructureRepository>();
        var mapperMock = new Mock<IMapper>();

        structureRepoMock
            .Setup(r => r.GetWithMembersAsync(999))
            .ReturnsAsync((GestionMateriel.Domain.Entities.Structure?)null);

        var handler = new GetStructureMembersQueryHandler(structureRepoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetStructureMembersQuery(999), CancellationToken.None);

        Assert.Empty(result);
        mapperMock.Verify(m => m.Map<GestionMateriel.Application.DTOs.Responses.StructureMemberResponse>(It.IsAny<object>()), Times.Never);
    }
}
