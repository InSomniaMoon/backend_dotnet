using GestionMateriel.Application.DTOs.Requests;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Commands;

public record UpdateItemCategoryCommand(int Id, UpdateItemCategoryRequest Request) : IRequest<ItemCategoryResponse?>;
