using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MSPApplication.Data;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace XUnitTestProject.Repositories
{
    public class UnitTestUserRepository :IDisposable
    {
        protected DbContextOptions<AppDbContext> ContextOptions { get; set; }
        private readonly DbConnection _connection;
        public UnitTestUserRepository()
        {
            ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            //.UseInMemoryDatabase("TestDatabase")
            //.UseSqlite(CreateInMemoryDatabase())
            .UseSqlite("Filename=Test.db")
            .Options;
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
            SeedData();
        }
        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        void SeedData()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var user1 = new AspNetUser { Id = Guid.NewGuid().ToString(), Email = "test1user@domain.co.uk", UserName = "test1user@domain.co.uk" };
                var user2 = new AspNetUser { Id = Guid.NewGuid().ToString(), Email = "test2user@domain.co.uk", UserName = "test2user@domain.co.uk" };
                var role1 = new AspNetRole { Id = Guid.NewGuid().ToString(), Name = "Production" };
                var role2 = new AspNetRole { Id = Guid.NewGuid().ToString(), Name = "Administration" };
                var role3 = new AspNetRole { Id = Guid.NewGuid().ToString(), Name = "Development" };
                var userRole1 = new AspNetUserRole { RoleId = role1.Id, UserId = user1.Id };
                var userRole2 = new AspNetUserRole { RoleId = role2.Id, UserId = user2.Id };
                var userRole3 = new AspNetUserRole { RoleId = role3.Id, UserId = user2.Id };
                context.AspNetUsers.AddRange(user1, user2);
                context.AspNetRoles.AddRange(role1, role2, role3);
                context.AspNetUserRoles.AddRange(userRole1, userRole2, userRole3);
                context.SaveChanges();
            }
        }
        [Fact]
        public void DeleteUserRole_TestDeleteOkay()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                var user = context.AspNetUsers.FirstOrDefault(e => e.UserName == "test2user@domain.co.uk");
                var role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                Assert.True(context.Set<AspNetUserRole>().Any(e => e.UserId == user.Id && e.RoleId == role.Id));
                userRepository.DeleteUserRole(user.Id, role.Id);
                Assert.False(context.Set<AspNetUserRole>().Any(e => e.UserId == user.Id && e.RoleId == role.Id));
            }
        }
        [Fact]
        public void GetUserRolesByIdsTest()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                var user = context.AspNetUsers.FirstOrDefault(e => e.UserName == "test2user@domain.co.uk");
                var role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                var result = userRepository.GetUserRoleByIds(user.Id, role.Id);
                Assert.Equal(user.Id, result.UserId);
                Assert.Equal(role.Id, result.RoleId);
                result = userRepository.GetUserRoleByIds("BadData", "BadData");
                Assert.Null(result);
            }
        }
        [Fact]
        public void GetAllUsers_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                List<AspNetUser> result = userRepository.GetAllUsers().ToList();
                Assert.Equal(2, result.Count());
                Assert.Contains("domain.co.uk", result[0].Email);
            }
        }
        [Fact]
        public void GetUserById_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                var result = userRepository.GetUserById("BadData");
                Assert.Null(result);
                var user = context.AspNetUsers.FirstOrDefault(e => e.Email == "test1user@domain.co.uk");
                result = userRepository.GetUserById(user.Id);
                Assert.NotNull(result);
            }
        }
        [Fact]
        public void AddUser_Test()
        {
            string emailAddress = "testuser@domain.co.uk";
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                var user = new AspNetUser { Email = emailAddress, UserName = emailAddress, Id = Guid.NewGuid().ToString() };
                var result = userRepository.AddUser(user);
                var savedResult = context.AspNetUsers.FirstOrDefault(e => e.Id == user.Id);
                Assert.Equal(savedResult.UserName, emailAddress);
                Exception exception = Assert.Throws<DbUpdateException>(() => result = userRepository.AddUser(user));
                Assert.Contains("See the inner exception for details", exception.Message);
            }
        }
        [Fact]
        public void UpdateUser_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                var user = context.AspNetUsers.Include(i => i.AspNetUserRoles).FirstOrDefault(e => e.UserName == "test1user@domain.co.uk");
                var role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                user.UserName = "test5user@domain.co.uk";
                AspNetUserRole userRole = new AspNetUserRole { UserId = user.Id, RoleId = role.Id };
                user.AspNetUserRoles.Add(userRole);
                var result1 = userRepository.UpdateUser(user);
                result1 = userRepository.GetUserById(user.Id);
                Assert.Equal("test5user@domain.co.uk", result1.UserName);
                var result2 = result1.AspNetUserRoles.FirstOrDefault(e => e.RoleId == role.Id);
                Assert.Equal(role.Id, result2.RoleId);
                Assert.Equal(user.Id, result2.UserId);
            }
        }
        [Fact]
        public void DeleteUser_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                var user = context.AspNetUsers.FirstOrDefault(e => e.UserName == "test2user@domain.co.uk");
                userRepository.DeleteUser(user.Id);
                user = context.AspNetUsers.FirstOrDefault(e => e.UserName == "test2user@domain.co.uk");
                Assert.Null(user);
            }
        }
        [Fact]
        public void TestInMemoryDatabase_AddingUnrelatedData()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var userRole = new AspNetUserRole { RoleId = "bad data", UserId = "bad data" };
                context.AspNetUserRoles.Add(userRole);
                Exception exception = Assert.Throws<DbUpdateException>(() => context.SaveChanges());
                Assert.Contains("See the inner exception for details", exception.Message);
                var result = context.AspNetUserRoles.FirstOrDefault(e => e.UserId == "bad data");
                Assert.Null(result);
            }
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}