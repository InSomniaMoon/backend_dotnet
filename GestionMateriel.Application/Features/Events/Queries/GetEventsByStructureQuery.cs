using GestionMateriel.Domain.Enums;

namespace GestionMateriel.Application.Features.Events.Queries;

public record GetEventsByStructureQuery(DateTime StartDate, DateTime EndDate, StructureTypeEnum StructureType, string StructureCode);
