namespace GestionMateriel.Domain.Entities;

public class EventSubscription
{
    // Composite key
    public int EventId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    
    // Navigation properties
    public Event Event { get; set; } = null!;
    public Item Item { get; set; } = null!;
}
