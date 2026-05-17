namespace GestionMateriel.Application.Services;

public interface ITenantProvider
{
    bool IsResolved { get; }
    int? StructureId { get; }
    string? StructureCode { get; }
    string? StructureMask { get; }

    void SetTenant(int? structureId, string? structureCode, string? structureMask);
}