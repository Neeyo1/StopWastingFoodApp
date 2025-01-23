using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Categories")]
public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }

    //Category - Product
    public ICollection<Product> Products { get; set; } = [];
}
