using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Category;

public class CategoryCreateDto
{
    [Required]
    public required string Name { get; set; }
}
