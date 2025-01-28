namespace API.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }

    //Product - Category
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    //Product - Inventory
    public ICollection<Inventory> Inventories { get; set; } = [];
}
