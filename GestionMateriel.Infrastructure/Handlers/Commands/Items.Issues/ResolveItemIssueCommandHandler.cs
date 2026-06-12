using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Infrastructure.Data;

namespace GestionMateriel.Infrastructure.Handlers.Commands.Items.Issues;

public class ResolveItemIssueCommandHandler(GestionMaterielDbContext db, IMapper mapper)
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
        return mapper.Map<ItemIssueResponse>(issue);
    }
}
