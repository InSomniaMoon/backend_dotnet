using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class CreateItemCommandHandler(IItemRepository itemRepository, IMapper mapper) : IRequestHandler<CreateItemCommand, ItemResponse>
{
    public async Task<ItemResponse> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var item = mapper.Map<Domain.Entities.Item>(command.Request);

        await itemRepository.AddAsync(item);
        await itemRepository.SaveChangesAsync();

        return mapper.Map<ItemResponse>(item);
    }
}
