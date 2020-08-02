using MSPApplication.Shared;
using System.Collections.Generic;

namespace MSPApplication.Data.Repositories
{
    public interface IRoleRepository
    {
        IEnumerable<AspNetRole> GetAllRoles();
        AspNetRole GetRoleById(string id);
        AspNetRole AddRole(AspNetRole role);
        AspNetRole UpdateRole(AspNetRole role);
        void DeleteRole(string id);
    }
}
