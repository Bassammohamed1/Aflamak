using BusinessLogicLayer.Helpers;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IRolesService
    {
        IEnumerable<IdentityRole> AllRoles();
        Task<Result> CreateRole(string roleName);
    }
}