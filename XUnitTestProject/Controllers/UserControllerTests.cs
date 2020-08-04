using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSPApplication.Api.Controllers;
using MSPApplication.Data;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;
using System;
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
        [Fact]
        public void TestGetUserById()
        {// Get all users should return a okay result
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                UserController sut = new UserController(userRepository);
                AspNetUser user = context.AspNetUsers.FirstOrDefault(e => e.Email == "test1user@domain.co.uk");
                IActionResult result = sut.GetUserById(user.Id);
                var okayResult = result as OkObjectResult;
                Assert.NotNull(okayResult);
                Assert.Equal(200, okayResult.StatusCode);
                AspNetUser userResult = okayResult.Value as AspNetUser;
                Assert.Equal("test1user@domain.co.uk", userResult.Email);
            }
        }
        [Fact]
        public void TestCreateUser()
        {// Get all users should return a okay result
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                UserController sut = new UserController(userRepository);
                AspNetUser user = new AspNetUser { Id = Guid.NewGuid().ToString(), Email = "testuser@domain.co.uk", UserName = "testuser@domain.co.uk" };
                IActionResult result = sut.CreateUser(user);
                var createdResult = result as CreatedResult;
                Assert.NotNull(createdResult);
                Assert.Equal(201, createdResult.StatusCode);
                AspNetUser userResult = createdResult.Value as AspNetUser;
                Assert.Equal("testuser@domain.co.uk", userResult.Email);
                Assert.Equal("testuser@domain.co.uk", userResult.UserName);
                result = sut.CreateUser(null);
                var badResult = result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
                user = new AspNetUser { Id = Guid.NewGuid().ToString(), Email = "testuser@domain.co.uk", UserName = string.Empty };
                result = sut.CreateUser(user);
                var bror = result as BadRequestObjectResult;
                Assert.Equal(400, bror.StatusCode);
                var modelState = sut.ModelState;
                Assert.Contains("UserName", modelState.Keys);
                Assert.True(modelState["UserName"].Errors.Count == 1);
                Assert.Equal("The Username should not be empty!", modelState["UserName"].Errors[0].ErrorMessage);
                Assert.False(modelState.IsValid);
            }
        }
        [Fact]
        public void TestUpdateUser()
        {// Get all users should return a okay result
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                UserController sut = new UserController(userRepository);
                AspNetUser user = context.AspNetUsers.FirstOrDefault(e => e.Email == "test1user@domain.co.uk");
                user.Email = "test7user@domain.co.uk";
                user.UserName = "test7user@domain.co.uk";
                IActionResult result = sut.UpdateUser(user);
                var noContent = result as NoContentResult;
                Assert.NotNull(noContent);
                Assert.Equal(204, noContent.StatusCode);
                result = sut.UpdateUser(null);
                var badResult = result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
                user = context.AspNetUsers.FirstOrDefault(e => e.Email == "test2user@domain.co.uk");
                user.Email = "test8user@domain.co.uk";
                user.UserName = string.Empty;
                result = sut.UpdateUser(user);
                var bror = result as BadRequestObjectResult;
                Assert.Equal(400, bror.StatusCode);
                var modelState = sut.ModelState;
                Assert.Contains("UserName", modelState.Keys);
                Assert.True(modelState["UserName"].Errors.Count == 1);
                Assert.Equal("The Username should not be empty!", modelState["UserName"].Errors[0].ErrorMessage);
                Assert.False(modelState.IsValid);
            }
        }
        [Fact]
        public void TestDeleteUser()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                UserController sut = new UserController(userRepository);
                AspNetUser user = context.AspNetUsers.FirstOrDefault(e => e.Email == "test1user@domain.co.uk");
                IActionResult result = sut.DeleteUser(user.Id);
                var noContent = result as NoContentResult;
                Assert.NotNull(noContent);
                Assert.Equal(204, noContent.StatusCode);
                result = sut.DeleteUser(null);
                var badResult = result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
                result = sut.DeleteUser("BadData");
                var notFound = result as NotFoundResult;
                Assert.Equal(404, notFound.StatusCode);

            }
        }
        [Fact]
        public void TestDeleteUserRole()
        {

            using (var context = new AppDbContext(ContextOptions))
            {
                UserRepository userRepository = new UserRepository(context);
                UserController sut = new UserController(userRepository);
                var user = context.AspNetUsers.FirstOrDefault(e => e.UserName == "test2user@domain.co.uk");
                var role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                Assert.True(context.Set<AspNetUserRole>().Any(e => e.UserId == user.Id && e.RoleId == role.Id));
                var result = sut.DeleteUserRole(user.Id, role.Id);
                Assert.False(context.Set<AspNetUserRole>().Any(e => e.UserId == user.Id && e.RoleId == role.Id));
                var okayResult = result as OkResult;
                Assert.NotNull(okayResult);
                Assert.Equal(200, okayResult.StatusCode);
                result = sut.DeleteUserRole(string.Empty, role.Id);
                var badRequest = result as BadRequestObjectResult;
                Assert.Equal(400, badRequest.StatusCode);
                result = sut.DeleteUserRole("BadData", "BadData");
                var notFound = result as NotFoundResult;
                Assert.Equal(404, notFound.StatusCode);

            }
        }
    }
}