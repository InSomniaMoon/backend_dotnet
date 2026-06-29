namespace GestionMateriel.Application.DTOs.Requests.Items;

public class GetPaginatedItemsRequest : PaginatedRequest
{
    public int? CategoryId { get; set; }
}
