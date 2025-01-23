namespace API.Entities;

public class Household
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreateDate { get; set; }

    //Household - AppUser
    public int OwnerId { get; set; }
    public AppUser Owner { get; set; } = null!;

    //Household - UserHousehold
    public ICollection<UserHousehold> UserHouseholds { get; set; } = [];

    //Household - ProductHousehold
    public ICollection<ProductHousehold> ProductHouseholds { get; set; } = [];
}
