using Aflamak.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aflamak.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.Select(u => new UsersVM
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = userManager.GetRolesAsync(u).Result

            }).ToListAsync();
            return View(users);
        }
        public async Task<IActionResult> ManageRoles(string userId)
        {
            if (userId == null)
                return NotFound("Invalid UserId");

            var user = await userManager.FindByIdAsync(userId);
            var roles = await roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesVM()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(r => new RolesVM
                {
                    Name = r.Name,
                    IsSelected = userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesVM data)
        {
            var user = await userManager.FindByIdAsync(data.UserId);
            if (user == null)
                return NotFound();

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in data.Roles)
            {
                if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                    await userManager.RemoveFromRoleAsync(user, role.Name);

                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                    await userManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
