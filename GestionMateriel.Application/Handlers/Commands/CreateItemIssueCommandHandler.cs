using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateItemIssueCommandHandler : IRequestHandler<CreateItemIssueCommand, ItemIssueResponse>
{
    private readonly IItemIssueRepository _itemIssueRepository;
    private readonly IMapper _mapper;

    public CreateItemIssueCommandHandler(IItemIssueRepository itemIssueRepository, IMapper mapper)
    {
        _itemIssueRepository = itemIssueRepository;
        _mapper = mapper;
    }

    public async Task<ItemIssueResponse> Handle(CreateItemIssueCommand command, CancellationToken cancellationToken)
    {
        var issue = _mapper.Map<ItemIssue>(command.Request);

        await _itemIssueRepository.AddAsync(issue);
        await _itemIssueRepository.SaveChangesAsync();

        return _mapper.Map<ItemIssueResponse>(issue);
    }
}
