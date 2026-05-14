using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetUserStructuresQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Null_When_User_Not_Found()
    {
        var repoMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetWithStructuresAsync(9)).ReturnsAsync((User?)null);

        var handler = new GetUserStructuresQueryHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetUserStructuresQuery(9), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Should_Return_Mapped_User_With_Structures_When_Found()
    {
        var repoMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        var user = new User { Id = 2, FirstName = "Paul", LastName = "M", Email = "paul@example.com" };

        repoMock.Setup(r => r.GetWithStructuresAsync(2)).ReturnsAsync(user);
        mapperMock.Setup(m => m.Map<UserWithStructuresResponse>(user)).Returns(new UserWithStructuresResponse { Id = 2, Email = "paul@example.com" });

        var handler = new GetUserStructuresQueryHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new GetUserStructuresQuery(2), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Id);
    }
}
