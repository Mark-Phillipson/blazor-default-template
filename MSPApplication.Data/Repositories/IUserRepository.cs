using MSPApplication.Shared;
using System.Collections.Generic;

namespace MSPApplication.Data.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<AspNetUser> GetAllUsers();
        AspNetUser GetUserById(string id);
        IEnumerable<AspNetUser> GetAllUsersInRole(string id);
        AspNetUserRole GetUserRoleByIds(string userId, string roleId);
        AspNetUser AddUser(AspNetUser user);
        AspNetUser UpdateUser(AspNetUser user);
        void DeleteUser(string id);
        void DeleteUserRole(string userId, string roleId);
    }
}
