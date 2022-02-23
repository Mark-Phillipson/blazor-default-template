using MSPApplication.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Services
{
    public interface IUserDataService
    {
        Task<AspNetUser> AddUser(AspNetUser user);
        Task DeleteUser(string id);
        Task<IEnumerable<AspNetUser>> GetAllUsers();
        Task<IEnumerable<AspNetUser>> GetAllUsersInRole(string id);
        Task<AspNetUser> GetUserById(string id);
        Task UpdateUser(AspNetUser user);
        Task DeleteUserRole(string userId, string roleId);
    }
}
