using AutoMapper;
using GestionMateriel.Application.Commands;
using GestionMateriel.Application.DTOs.Responses;
using GestionMateriel.Domain.Interfaces;
using MediatR;

namespace GestionMateriel.Application.Handlers.Commands;

public class UpdateItemCategoryCommandHandler : IRequestHandler<UpdateItemCategoryCommand, ItemCategoryResponse?>
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IMapper _mapper;

    public UpdateItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository, IMapper mapper)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _mapper = mapper;
    }

    public async Task<ItemCategoryResponse?> Handle(UpdateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _itemCategoryRepository.GetByIdAsync(command.Id);
        if (category is null)
        {
            return null;
        }

        _mapper.Map(command.Request, category);
        category.UpdatedAt = DateTime.UtcNow;

        await _itemCategoryRepository.UpdateAsync(category);
        await _itemCategoryRepository.SaveChangesAsync();

        return _mapper.Map<ItemCategoryResponse>(category);
    }
}
