using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Product;

public class ProductCreateDto
{
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
}
