using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSPApplication.Api.Controllers;
using MSPApplication.Data;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Controllers.Tests
{
    public class UserControllerTests
    {
        protected DbContextOptions<AppDbContext> ContextOptions { get; set; }

        public UserControllerTests()
        {
            ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
            SeedData();
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
        public void TestGetAllUsers()
        {// Get all users should return a okay result
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);

                UserController sut = new UserController(userRepository);
                IActionResult result = sut.GetAllUsers();
                var okayResult = result as OkObjectResult;
                Assert.NotNull(okayResult);
                Assert.Equal(200, okayResult.StatusCode);
                IEnumerable<AspNetUser> users = okayResult.Value as IEnumerable<AspNetUser>;
                Assert.Equal(2, users.Count());
                var user = users.FirstOrDefault(e => e.Email == "test1user@domain.co.uk");
                Assert.Equal("test1user@domain.co.uk", user.Email);
            }
        }
    }
}