using MSPApplication.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplication.UI.Services
{
    public interface IUserDataService
    {
        Task<AspNetUser> AddUser(AspNetUser user);
        Task DeleteUser(string id);
        Task<IEnumerable<AspNetUser>> GetAllUsers();
        Task<AspNetUser> GetUserById(string id);
        Task UpdateUser(AspNetUser user);
    }
}