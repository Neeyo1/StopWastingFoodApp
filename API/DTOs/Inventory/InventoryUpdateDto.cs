using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Inventory;

public class InventoryUpdateDto
{
    [Required]
    public required DateTime ExpireDate { get; set; }
}
