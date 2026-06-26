namespace GestionMateriel.Application.DTOs.Responses;

public class EventResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int StructureId { get; set; }

    public int? CreatedById { get; set; }
    public UserResponse? CreatedBy { get; set; }
    public string? Comment { get; set; }
    public required StructureResponse Structure { get; set; }

}

public class EventWithItemsResponse : EventResponse
{
    public IEnumerable<ItemWithQuantityResponse> Items { get; set; } = [];
}
