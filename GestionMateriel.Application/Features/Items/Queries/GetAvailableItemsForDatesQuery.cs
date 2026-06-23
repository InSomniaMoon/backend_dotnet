namespace GestionMateriel.Application.Features.Items.Queries;

public record GetAvailableItemsForDatesQuery(int Page, int Size, string? OrderBy, string? OrderDir, DateTime StartDate, DateTime EndDate, string? Q = null, int? ForEventId = null, int? CategoryId = null)
{
    private static readonly string[] AllowedOrderBy = ["items.name", "categoryId"];
    public void Validate()
    {
        if (!AllowedOrderBy.Contains(OrderBy))
        {
            throw new ArgumentException($"OrderBy doit etre l'un des suivants : {string.Join(", ", AllowedOrderBy)}");
        }


        if (StartDate > EndDate)
        {
            throw new ArgumentException("StartDate doit etre inferieur ou egal a EndDate");
        }

    }
}