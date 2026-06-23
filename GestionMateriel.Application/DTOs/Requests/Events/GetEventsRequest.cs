using System.ComponentModel.DataAnnotations;

namespace GestionMateriel.Application.DTOs.Requests.Events;

public class GetEventsRequest
{
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}
