using System.ComponentModel.DataAnnotations.Schema;

namespace GestionMateriel.Domain.Entities;

[Table("event_subscriptions")]
public class EventSubscription
{
    // Composite key
    [Column("event_id")]
    public int EventId { get; set; }
    [Column("item_id")]
    public int ItemId { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }

    // Navigation properties
    public virtual Event Event { get; set; } = null!;
    public virtual Item Item { get; set; } = null!;
}
