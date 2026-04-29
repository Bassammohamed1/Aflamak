using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IEnumerable<UsersDTO> AllUsers()
        {
            return _userManager.Users.Select(u => new UsersDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result

            }).ToList();
        }

        public async Task<UserRolesDTO> GetAllRolesWithUserSelectedRoles(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            var roles = _roleManager.Roles.ToList();

            var data = new UserRolesDTO()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(r => new RolesDTO
                {
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };

            return data;
        }

        public async Task<Result> ManageRoles(UserRolesDTO data)
        {
            var user = await _userManager.FindByIdAsync(data.UserId);

            if (user is null)
                return new Result()
                {
                    Success = false,
                    Error = "User is null."
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in data.Roles)
            {
                if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }

            return new Result() { Success = true };
        }
    }
}
