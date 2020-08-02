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

        public AspNetUser GetUserById(string id)
        {
            return _appDbContext.AspNetUsers.Include(i => i.AspNetUserLogins).FirstOrDefault(c => c.Id == id);
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
                foundUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

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
    }
}
