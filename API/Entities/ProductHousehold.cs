namespace API.Entities;

public class ProductHousehold
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int HouseholdId { get; set; }
    public Household Household { get; set; } = null!;

    public DateTime BuyDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public FreshStatus Status { get; set; }
}
