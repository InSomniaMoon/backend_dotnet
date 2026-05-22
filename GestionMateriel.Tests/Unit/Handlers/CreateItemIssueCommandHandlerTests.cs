using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Items.Issues;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Handlers.Commands.Items.Issues;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Moq;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateItemIssueCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Issue_And_Return_Response()
    {
        var repoMock = new Mock<IItemIssueRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new CreateItemIssueRequest
        {
            ItemId = 1,
            Value = "Zip casse",
            ReportedById = 2,
            AffectedQuantity = 1
        };

        var issue = new ItemIssue { Id = 5, ItemId = 1, Value = "Zip casse", ReportedBy = 2, AffectedQuantity = 1 };

        mapperMock.Setup(m => m.Map<ItemIssue>(request)).Returns(issue);
        mapperMock.Setup(m => m.Map<ItemIssueResponse>(issue)).Returns(new ItemIssueResponse { Id = 5, ItemId = 1 });

        var handler = new CreateItemIssueCommandHandler(repoMock.Object, mapperMock.Object);

        var result = await handler.Handle(new CreateItemIssueCommand(request), CancellationToken.None);

        Assert.Equal(5, result.Id);
        repoMock.Verify(r => r.AddAsync(issue), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
