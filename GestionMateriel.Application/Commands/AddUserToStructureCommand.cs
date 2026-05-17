using GestionMateriel.Application.DTOs.Requests.Structures;
using GestionMateriel.Application.DTOs.Responses;
using MediatR;

namespace GestionMateriel.Application.Commands;

public record AddUserToStructureCommand(AddUserToStructureRequest Request) : IRequest<StructureMemberResponse?>;
