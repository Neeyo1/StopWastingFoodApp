using API.DTOs.Product;
using API.Entities;

namespace API.DTOs.Inventory;

public class InventoryDto
{
    public int Id { get; set; }
    public ProductDto Product { get; set; } = null!;
    public DateTime BuyDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public required string Status { get; set; }
}
