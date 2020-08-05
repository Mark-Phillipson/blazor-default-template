using Microsoft.EntityFrameworkCore;
using MSPApplication.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MSPApplication.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<AspNetUser> GetAllUsers()
        {
            return _appDbContext.AspNetUsers;
        }
        public IEnumerable<AspNetUser> GetAllUsersInRole(string roleId)
        {
            var userRoles = _appDbContext.AspNetUserRoles.Where(v => v.RoleId == roleId);
            List<AspNetUser> result = new List<AspNetUser>();
            foreach (var userRole in userRoles)
            {
                AspNetUser user = _appDbContext.AspNetUsers.Where(v => v.Id == userRole.UserId).FirstOrDefault();
                result.Add(user);
            }
            return result;
        }

        public AspNetUser GetUserById(string id)
        {
            return _appDbContext.AspNetUsers.Include("AspNetUserRoles.Role").FirstOrDefault(c => c.Id == id);
        }

        public AspNetUser AddUser(AspNetUser user)
        {
            var addedEntity = _appDbContext.AspNetUsers.Add(user);
            _appDbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public AspNetUser UpdateUser(AspNetUser user)
        {
            var foundUser = _appDbContext.AspNetUsers.FirstOrDefault(e => e.Id == user.Id);

            if (foundUser != null)
            {
                foundUser.EmailConfirmed = user.EmailConfirmed;
                foundUser.LockoutEnabled = user.LockoutEnabled;
                foundUser.LockoutEnd = user.LockoutEnd;
                foundUser.PhoneNumber = user.PhoneNumber;
                foundUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

                foreach (var userRole in user.AspNetUserRoles)
                {
                    userRole.Role = null;
                    var foundUserRole = _appDbContext.AspNetUserRoles.FirstOrDefault(e => e.RoleId == userRole.RoleId && e.UserId == userRole.UserId);
                    if (foundUserRole == null)
                    {
                        _appDbContext.AspNetUserRoles.Add(userRole);
                        _appDbContext.SaveChanges();
                    }
                }
                _appDbContext.AspNetUsers.Update(foundUser);
                _appDbContext.SaveChanges();
                return foundUser;
            }

            return null;
        }

        public void DeleteUser(string id)
        {
            var foundUser = _appDbContext.AspNetUsers.FirstOrDefault(e => e.Id == id);
            if (foundUser == null) return;

            _appDbContext.AspNetUsers.Remove(foundUser);
            _appDbContext.SaveChanges();
        }


        public void DeleteUserRole(string userId, string roleId)
        {
            var foundUserRole = _appDbContext.AspNetUserRoles.FirstOrDefault(e => e.UserId == userId && e.RoleId == roleId);
            if (foundUserRole == null) return;

            _appDbContext.AspNetUserRoles.Remove(foundUserRole);
            _appDbContext.SaveChanges();
        }
        public AspNetUserRole GetUserRoleByIds(string userId, string roleId)
        {
            AspNetUserRole result = _appDbContext.AspNetUserRoles.FirstOrDefault(e => e.UserId == userId && e.RoleId == roleId);
            return result;
        }
    }
}
