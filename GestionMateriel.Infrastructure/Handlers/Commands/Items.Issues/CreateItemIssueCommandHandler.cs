using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

public class CreateItemIssueCommandHandler(GestionMaterielDbContext db, IMapper mapper)
    : IRequestHandler<CreateItemIssueCommand, ItemIssueResponse>
{
    public async Task<ItemIssueResponse> Handle(CreateItemIssueCommand command, CancellationToken cancellationToken)
    {
        var issue = new ItemIssue
        {
            ItemId = command.ItemId,
            AffectedQuantity = command.Request.AffectedQuantity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReportedBy = command.ReportedById,
            Value = command.Request.Value,
        };
        await db.ItemIssues.AddAsync(issue, cancellationToken);

        if (!command.Request.Usable)
        {
            var item = await db.Items.FindAsync([command.ItemId], cancellationToken);
            item?.Usable = false;
        }

        await db.SaveChangesAsync(cancellationToken);
        return mapper.Map<ItemIssueResponse>(issue);
    }
}
