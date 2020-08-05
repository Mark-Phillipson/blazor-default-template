using Microsoft.EntityFrameworkCore;
using MSPApplication.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSPApplication.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _appDbContext;

        public RoleRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<AspNetRole> GetAllRoles()
        {
            return _appDbContext.AspNetRoles;
        }

        public AspNetRole GetRoleById(string id)
        {
            return _appDbContext.AspNetRoles.Include(i => i.AspNetUserRoles).FirstOrDefault(c => c.Id == id);
        }

        public AspNetRole AddRole(AspNetRole role)
        {
            var existingRole = _appDbContext.AspNetRoles.FirstOrDefault(e => e.Id == role.Id);
            if (string.IsNullOrEmpty(role.Id) || existingRole != null)
            {
                role.Id = Guid.NewGuid().ToString();
            }
            existingRole = _appDbContext.AspNetRoles.FirstOrDefault(e => e.Name.ToLower() == role.Name.ToLower());
            if (existingRole != null)
            {
                return null;
            }
            role.NormalizedName = role.Name.ToUpper();
            var addedEntity = _appDbContext.AspNetRoles.Add(role);
            _appDbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public AspNetRole UpdateRole(AspNetRole role)
        {
            var foundRole = _appDbContext.AspNetRoles.FirstOrDefault(e => e.Id == role.Id);

            if (foundRole != null)
            {
                foundRole.ConcurrencyStamp = role.ConcurrencyStamp;
                foundRole.Name = role.Name;
                foundRole.NormalizedName = role.Name.ToUpper();
                _appDbContext.SaveChanges();
                return foundRole;
            }
            return null;
        }

        public void DeleteRole(string id)
        {
            var foundRole = _appDbContext.AspNetRoles.FirstOrDefault(e => e.Id == id);
            if (foundRole == null) return;

            _appDbContext.AspNetRoles.Remove(foundRole);
            _appDbContext.SaveChanges();
        }
    }
}
