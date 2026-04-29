using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public IActionResult Index()
        {
            var roles = _rolesService.AllRoles();

            return View(roles);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(RoleFormVM data)
        {
            if (ModelState.IsValid)
            {
                var result = await _rolesService.CreateRole(data.Name);

                if (result.Success)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("Name", result.Error);
                    return View("Index", _rolesService.AllRoles());
                }
            }
            else
            {
                return View("Index", _rolesService.AllRoles());
            }
        }
    }
}
