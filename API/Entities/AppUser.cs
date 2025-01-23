using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    public required string KnownAs { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime LastActiveDate { get; set; } = DateTime.UtcNow;

    //AppUser - AppUserRole
    public ICollection<AppUserRole> UserRoles { get; set; } = [];

    //AppUser - Household
    public ICollection<Household> Households { get; set; } = [];

    //AppUser - UserHousehold
    public ICollection<UserHousehold> UserHouseholds { get; set; } = [];
}
