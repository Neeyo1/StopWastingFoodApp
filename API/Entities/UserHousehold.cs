namespace API.Entities;

public class UserHousehold
{
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public int HouseholdId { get; set; }
    public Household Household { get; set; } = null!;
}
