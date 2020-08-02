using MSPApplication.Shared;
using System.Collections.Generic;

namespace MSPApplication.Data.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<AspNetUser> GetAllUsers();
        AspNetUser GetUserById(string id);
        AspNetUser AddUser(AspNetUser user);
        AspNetUser UpdateUser(AspNetUser user);
        void DeleteUser(string id);
    }
}
