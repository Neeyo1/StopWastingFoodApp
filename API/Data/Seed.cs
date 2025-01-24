using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedData(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        ILogger<Seed> logger)
    {
        if (await roleManager.Roles.AnyAsync())
        {
            logger.LogInformation("----- Roles aleady existing, seeding process skipped -----");
        }
        else
        {
            var roles = new List<AppRole>
            {
                new() {Name = "User"},
                new() {Name = "Admin"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }

        if (await userManager.Users.FirstOrDefaultAsync(x => x.UserName == "admin") != null)
        {
            logger.LogInformation("----- Admin user aleady existing, seeding process skipped -----");
        }
        else
        {
            var admin = new AppUser
            {
                UserName = "admin",
                KnownAs = "Admin"
            };

            await userManager.CreateAsync(admin, "zaq1@WSX");
            await userManager.AddToRolesAsync(admin, ["Admin"]);
        }
    }
}
