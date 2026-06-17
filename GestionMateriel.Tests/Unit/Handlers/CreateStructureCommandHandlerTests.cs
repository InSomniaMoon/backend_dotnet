using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Commands.Structures;

namespace GestionMateriel.Tests.Unit.Handlers;

public class CreateStructureCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Structure()
    {
        using var db = TestHelper.CreateDbContext();
        var handler = new CreateStructureCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new CreateStructureCommand(new CreateStructureRequest
        {
            Name = "Groupe Local",
            CodeStructure = "GL123",
            Type = StructureTypeEnum.Groupe.ToString()
        }), CancellationToken.None);
        Assert.Equal("Groupe Local", result.Name);
        Assert.Equal(1, db.Structures.Count());
    }
}
