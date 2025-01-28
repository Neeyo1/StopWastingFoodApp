using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Inventories")]
public class Inventory
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int HouseholdId { get; set; }
    public Household Household { get; set; } = null!;

    public DateTime BuyDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpireDate { get; set; }
    public FreshStatus Status { get; set; }
}
