using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Controllers
{
    public class UsersController : Controller
    {

        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var users = _usersService.AllUsers();

            return View(users);
        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            if (userId == null)
                return NotFound("Invalid userID");

            var result = await _usersService.GetAllRolesWithUserSelectedRoles(userId);

            var viewModel = new UserRolesVM()
            {
                UserId = result.UserId,
                UserName = result.UserName,
                Roles = result.Roles.Select(r => new RolesVM
                {
                    Name = r.Name,
                    IsSelected = r.IsSelected
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesVM data)
        {
            var dto = new UserRolesDTO()
            {
                UserId = data.UserId,
                UserName = data.UserName,
                Roles = data.Roles.Select(r => new RolesDTO() { Name = r.Name, IsSelected = r.IsSelected }).ToList()
            };

            var result = await _usersService.ManageRoles(dto);

            return result.Success ? RedirectToAction(nameof(Index)) : View(data);

        }
    }
}
