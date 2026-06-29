using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

public class ResolveItemIssueCommandHandler(GestionMaterielDbContext db)
    : IRequestHandler<ResolveItemIssueCommand, ItemIssueResponse?>
{
    public async Task<ItemIssueResponse?> Handle(ResolveItemIssueCommand command, CancellationToken cancellationToken)
    {
        var issue = await db.ItemIssues.FindAsync([command.Id], cancellationToken);
        if (issue is null) return null;

        issue.Status = IssueStatusEnum.Resolved;
        issue.ResolutionDate = DateTime.UtcNow;
        issue.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        var item = await db.Items.FindAsync([issue.ItemId], cancellationToken) ?? throw new KeyNotFoundException($"Item with ID {issue.ItemId} not found.");

        if (!item.Issues.Any(i => i.Status == IssueStatusEnum.Open))
        {
            item.Usable = true;
            item.State = ItemState.OK;
        }
        else if (item.Issues.Any(i => i.Status == IssueStatusEnum.Open && !i.IsItemUsable))
        {
            item.Usable = false;
            item.State = ItemState.KO;
        }
        else
        {
            item.Usable = true;
            item.State = ItemState.NOK;
        }



        await db.SaveChangesAsync(cancellationToken);
        return new ItemIssueResponse
        {
            Id = issue.Id,
            ItemId = issue.ItemId,
            CreatedAt = issue.CreatedAt,
            Status = issue.Status.ToString(),
            Value = issue.Value,
            ReportedById = issue.ReportedBy,
            AffectedQuantity = issue.AffectedQuantity,
            ResolutionDate = issue.ResolutionDate,
        };
    }
}
