using AutoMapper;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Queries.Users;
using GestionMateriel.Application.Queries;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class GetUsersQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Users()
    {
        var repoMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>
        {
            new() { Id = 1, FirstName = "Ada", LastName = "L", Email = "ada@example.com", Role = RoleEnum.Admin }
        });

        mapperMock.Setup(m => m.Map<UserResponse>(It.IsAny<User>()))
            .Returns<User>(u => new UserResponse { Id = u.Id, Email = u.Email, Role = u.Role.ToString() });

        var handler = new GetUsersQueryHandler(repoMock.Object, mapperMock.Object);

        var result = (await handler.Handle(new GetUsersQuery(), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("ada@example.com", result[0].Email);
    }
}
