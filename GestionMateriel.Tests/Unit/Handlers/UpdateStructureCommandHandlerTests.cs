using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Application.Features.Structures.Commands;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Handlers.Commands.Structures;

namespace GestionMateriel.Tests.Unit.Handlers;

public class UpdateStructureCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Structure()
    {
        using var db = TestHelper.CreateDbContext();
        db.Structures.Add(new Structure { Id = 1, Name = "Old", CodeStructure = "GL1", Type = StructureTypeEnum.Groupe });
        await db.SaveChangesAsync();
        var handler = new UpdateStructureCommandHandler(db, TestHelper.CreateMapper());
        var result = await handler.Handle(new UpdateStructureCommand(1, "NewColor", "New", [1, 2, 3]), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("New", result!.Name);
    }
}
