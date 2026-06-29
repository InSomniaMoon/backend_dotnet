using GestionMateriel.Application.Services;

namespace GestionMateriel.Presentation.Middleware;

public class TenantContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
    {
        var structureIdClaim = context.User.FindFirst("structureId")?.Value;
        int? structureId = int.TryParse(structureIdClaim, out var parsedStructureId) ? parsedStructureId : null;

        var structureCode = Normalize(context.User.FindFirst("structureCode")?.Value);
        var structureMask = Normalize(context.User.FindFirst("structureMask")?.Value);
        var isAppAdmin = context.User.FindFirst("app_role")?.Value == "admin";

        tenantProvider.SetTenant(structureId, structureCode, structureMask, isAppAdmin);

        await next(context);
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}