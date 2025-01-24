using API.DTOs.Category;

namespace API.DTOs.Product;

public class ProductDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public CategoryDto Category { get; set; } = null!;
}
