using MSPApplication.Shared;
using MSPApplication.Shared.ViewModels;
using System.Collections.Generic;

namespace MSPApplication.Data.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(string id);
        IEnumerable<User> GetAllUsersInRole(string id);
        AspNetUserRole GetUserRoleByIds(string userId, string roleId);
        AspNetUser AddUser(AspNetUser user);
        AspNetUser UpdateUser(AspNetUser user);
        void DeleteUser(string id);
        void DeleteUserRole(string userId, string roleId);
        IEnumerable<AspNetRole> GetAllRolesForUser(string  userId);
        AspNetUserRole AddUserRole(AspNetUserRole userRole);
    }
}
