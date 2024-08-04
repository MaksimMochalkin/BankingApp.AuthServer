namespace BankingApp.AuthServer.AppConfiguration
{
    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;

    public class RoleManager
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
                var temp = await roleManager.FindByNameAsync("Administrator");

                await roleManager.AddClaimAsync(temp, new Claim(ClaimTypes.Role, "Administrator"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
                var temp = await roleManager.FindByNameAsync("User");
                await roleManager.AddClaimAsync(temp, new Claim(ClaimTypes.Role, "User"));

            }
        }
    }
}
