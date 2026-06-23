using System.ComponentModel.DataAnnotations;
using GestionMateriel.Application.DTOs.Requests;

namespace GestionMateriel.Application.Features.Items.Requests;

[CustomValidation(typeof(GetAvailableItemsForDatesRequest), nameof(ValidateDateRange))]
public class GetAvailableItemsForDatesRequest : PaginatedRequest
{
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    public int? ForEventId { get; set; }
    public int? CategoryId { get; set; }

    public static ValidationResult ValidateDateRange(GetAvailableItemsForDatesRequest request, ValidationContext context)
    {
        if (request.StartDate > request.EndDate)
        {
            return new ValidationResult("StartDate must be less than or equal to EndDate.");
        }
        return ValidationResult.Success!;
    }
}