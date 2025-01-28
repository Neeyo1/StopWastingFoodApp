using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Inventory;

public class InventoryCreateDto
{
    [Required]
    public required int ProductId { get; set; }

    [Required]
    public required int HouseholdId { get; set; }

    [Required]
    public required DateTime ExpireDate { get; set; }
}
