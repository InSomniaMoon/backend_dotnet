using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Users;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_User_When_Email_Not_Used()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new CreateUserRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com",
            Password = "password123",
            Role = "admin"
        };

        userRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        mapperMock
            .Setup(m => m.Map<UserResponse>(It.IsAny<User>()))
            .Returns<User>(u => new UserResponse { Id = u.Id, Email = u.Email, Role = u.Role.ToString() });

        var handler = new CreateUserCommandHandler(userRepoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new CreateUserCommand(request), CancellationToken.None);

        Assert.Equal("ada@example.com", result.Email);
        userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Email == request.Email &&
            u.Role == RoleEnum.Admin &&
            !string.IsNullOrWhiteSpace(u.Password) &&
            u.Password != request.Password)), Times.Once);
        userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
