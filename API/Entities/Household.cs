namespace API.Entities;

public class Household
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    //Household - AppUser
    public int OwnerId { get; set; }
    public AppUser Owner { get; set; } = null!;

    //Household - UserHousehold
    public ICollection<UserHousehold> UserHouseholds { get; set; } = [];

    //Household - Inventory
    public ICollection<Inventory> Inventories { get; set; } = [];
}
