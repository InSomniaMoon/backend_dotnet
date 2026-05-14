using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Enums;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class ResolveItemIssueCommandHandler : IRequestHandler<ResolveItemIssueCommand, ItemIssueResponse?>
{
    private readonly IItemIssueRepository _itemIssueRepository;
    private readonly IMapper _mapper;

    public ResolveItemIssueCommandHandler(IItemIssueRepository itemIssueRepository, IMapper mapper)
    {
        _itemIssueRepository = itemIssueRepository;
        _mapper = mapper;
    }

    public async Task<ItemIssueResponse?> Handle(ResolveItemIssueCommand command, CancellationToken cancellationToken)
    {
        var issue = await _itemIssueRepository.GetByIdAsync(command.Id);
        if (issue is null)
        {
            return null;
        }

        issue.Status = IssueStatusEnum.Resolved;
        issue.ResolutionDate = DateTime.UtcNow;
        issue.UpdatedAt = DateTime.UtcNow;

        await _itemIssueRepository.UpdateAsync(issue);
        await _itemIssueRepository.SaveChangesAsync();

        return _mapper.Map<ItemIssueResponse>(issue);
    }
}
