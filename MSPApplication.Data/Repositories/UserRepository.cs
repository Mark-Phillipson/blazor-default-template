using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MSPApplication.Shared;
using MSPApplication.Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MSPApplication.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _appDbContext;
		readonly IMapper _mapper;

		public UserRepository(AppDbContext  appDbContext, IMapper mapper)
		{
			this._mapper = mapper;
			_appDbContext = appDbContext;
		}

		public IEnumerable<User> GetAllUsers()
		{

			var users = _appDbContext.AspNetUsers;
			List<User> usersVM = new List<User>();
			foreach (var user in users)
			{
				var userVM = _mapper.Map<AspNetUser,User>(user);
				usersVM.Add(userVM);
			}
			return usersVM;

		}
		public IEnumerable<User> GetAllUsersInRole(string roleId)
		{
			var userRoles = _appDbContext.AspNetUserRoles.Where(v => v.RoleId == roleId);
			List<User> result = new List<User>();
			foreach (var userRole in userRoles)
			{
				AspNetUser user = _appDbContext.AspNetUsers.Where(v => v.Id == userRole.UserId).FirstOrDefault();
				var userVM = _mapper.Map<AspNetUser, User>(user);
				result.Add(userVM);
			}
			return result;
		}

		public User GetUserById(string id)
		{
			var user= _appDbContext.AspNetUsers.Include("AspNetUserRoles.Role").FirstOrDefault(c => c.Id == id);
			var userVM = _mapper.Map<AspNetUser, User>(user);
			return userVM;

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
