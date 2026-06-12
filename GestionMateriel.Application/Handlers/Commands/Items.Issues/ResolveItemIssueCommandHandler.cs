using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Application.Messaging;

namespace GestionMateriel.Application.Handlers.Commands.Items.Issues;

public class ResolveItemIssueCommandHandler(IItemIssueRepository itemIssueRepository, IMapper mapper) : IRequestHandler<ResolveItemIssueCommand, ItemIssueResponse?>
{
    public async Task<ItemIssueResponse?> Handle(ResolveItemIssueCommand command, CancellationToken cancellationToken)
    {
        var issue = await itemIssueRepository.GetByIdAsync(command.Id);
        if (issue is null)
        {
            return null;
        }

        issue.Status = IssueStatusEnum.Resolved;
        issue.ResolutionDate = DateTime.UtcNow;
        issue.UpdatedAt = DateTime.UtcNow;

        await itemIssueRepository.UpdateAsync(issue);
        await itemIssueRepository.SaveChangesAsync();

        return mapper.Map<ItemIssueResponse>(issue);
    }
}
