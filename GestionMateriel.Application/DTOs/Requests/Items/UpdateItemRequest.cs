using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Items;

public class UpdateItemRequest
{
    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [MaxLength(1000)] public string? Description { get; set; }

    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
    public bool Usable { get; set; }

    [Range(0, int.MaxValue)] public int Stock { get; set; }

    public DateTime? DateOfBuy { get; set; }

    [MaxLength(500)] public string? Image { get; set; }

    public override string ToString()
    {
        return
            $"UpdateItemRequest {{ Name = {Name}, Description = {Description}, CategoryId = {CategoryId}, Usable = {Usable}, Stock = {Stock}, DateOfBuy = {DateOfBuy}, Image = {Image} }}";
    }
}
