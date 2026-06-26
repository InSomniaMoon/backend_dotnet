using System.Text.Json;
using GestionMateriel.Application.DTOs.Common;

namespace GestionMateriel.Presentation.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Erreur de règle métier");
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Accès non autorisé");
            await WriteErrorAsync(context, StatusCodes.Status401Unauthorized, "Accès non autorisé.");
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Ressource non trouvée");
            await WriteErrorAsync(context, StatusCodes.Status404NotFound, "Ressource non trouvée.", ex.Message);
        }
        catch (OperationCanceledException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status499ClientClosedRequest, "Opération annulée par le client.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception non gérée");
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "Une erreur inattendue est survenue.", ex.Message);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message, string? details = null)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            Details = details
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload, JsonOptions));
    }
}
