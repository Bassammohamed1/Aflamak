using DataAccessLayer.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(RoleFormVM data)
        {
            if (ModelState.IsValid)
            {
                if (await roleManager.RoleExistsAsync(data.Name))
                {
                    ModelState.AddModelError("Name", "هذا الدور موجود بالفعل.");
                    return View("Index", roleManager.Roles);
                }
                else
                {
                    await roleManager.CreateAsync(new IdentityRole(data.Name.Trim()));
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View("Index", roleManager.Roles);
            }
        }
    }
}
