namespace GestionMateriel.Application.Services;

public interface ITenantProvider
{
    bool IsResolved { get; }
    int? StructureId { get; }
    string? StructureCode { get; }
    string? StructureMask { get; }
    bool IsAppAdmin { get; }

    void SetTenant(int? structureId, string? structureCode, string? structureMask, bool isAppAdmin = false);
}