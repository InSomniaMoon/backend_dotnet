using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class AddUserToStructureCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_User_Or_Structure_Not_Found()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var structureRepoMock = new Mock<IStructureRepository>();
        var userStructureRepoMock = new Mock<IUserStructureRepository>();
        var mapperMock = new Mock<IMapper>();

        userRepoMock
            .Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync((GestionMateriel.Domain.Entities.User?)null);

        structureRepoMock
            .Setup(r => r.GetByIdAsync(20))
            .ReturnsAsync(new GestionMateriel.Domain.Entities.Structure { Id = 20, Name = "Groupe A", CodeStructure = "GA", NomStructure = "Groupe A" });

        var handler = new AddUserToStructureCommandHandler(
            userRepoMock.Object,
            structureRepoMock.Object,
            userStructureRepoMock.Object,
            mapperMock.Object);

        var result = await handler.Handle(
            new AddUserToStructureCommand(new AddUserToStructureRequest
            {
                UserId = 10,
                StructureId = 20,
                Role = "User"
            }),
            CancellationToken.None);

        Assert.Null(result);
        userStructureRepoMock.Verify(r => r.AddAsync(It.IsAny<GestionMateriel.Domain.Entities.UserStructure>()), Times.Never);
    }
}
