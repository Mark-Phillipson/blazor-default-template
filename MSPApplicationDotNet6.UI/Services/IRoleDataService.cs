using MSPApplication.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Services
{
    public interface IRoleDataService
    {
        Task<AspNetRole> AddRole(AspNetRole role);
        Task DeleteRole(string id);
        Task<IEnumerable<AspNetRole>> GetAllRoles();
        Task<AspNetRole> GetRoleById(string id);
        Task UpdateRole(AspNetRole role);
    }
}