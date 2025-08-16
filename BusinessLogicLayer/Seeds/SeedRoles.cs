using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Seeds
{
    public static class SeedRoles
    {
        public static async Task seedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("Admin") == null && await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }
    }
}
