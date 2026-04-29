using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IUsersService
    {
        IEnumerable<UsersDTO> AllUsers();
        Task<UserRolesDTO> GetAllRolesWithUserSelectedRoles(string userID);
        Task<Result> ManageRoles(UserRolesDTO data);
    }
}