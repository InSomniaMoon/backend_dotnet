using GestionMateriel.Application.Services;

namespace GestionMateriel.Infrastructure.Services;

public class TenantProvider : ITenantProvider
{
    public bool IsResolved { get; private set; }
    public int? StructureId { get; private set; }
    public string? StructureCode { get; private set; }
    public string? StructureMask { get; private set; }
    public bool IsAppAdmin { get; private set; }

    public void SetTenant(int? structureId, string? structureCode, string? structureMask, bool isAppAdmin = false)
    {
        StructureId = structureId;
        StructureCode = string.IsNullOrWhiteSpace(structureCode) ? null : structureCode;
        StructureMask = string.IsNullOrWhiteSpace(structureMask) ? null : structureMask;
        IsAppAdmin = isAppAdmin;
        IsResolved = StructureId.HasValue || StructureCode is not null || StructureMask is not null;
    }
}