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
    public class RoleControllerTests
    {
        protected DbContextOptions<AppDbContext> ContextOptions { get; set; }

        public RoleControllerTests()
        {
            ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
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
                if (context.AspNetUsers.Count() != 2 || context.AspNetRoles.Count() != 3 || context.AspNetUserRoles.Count() != 3)
                {
                    throw new Exception(" database was not deleted correctly! ");
                }
            }
        }

        [Fact]
        public void TestGetAllRoles()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                RoleController sut = new RoleController(roleRepository);
                IActionResult result = sut.GetAllRoles();
                var okayResult = result as OkObjectResult;
                Assert.NotNull(okayResult);
                Assert.Equal(200, okayResult.StatusCode);
                IEnumerable<AspNetRole> roles = okayResult.Value as IEnumerable<AspNetRole>;
                Assert.Equal(3, roles.Count());
                var role = roles.FirstOrDefault(e => e.Name == "Production");
                Assert.Equal("Production", role.Name);
            }
        }
        [Fact]
        public void TestGetRoleById()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                RoleController sut = new RoleController(roleRepository);
                AspNetRole role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Production");
                IActionResult result = sut.GetRoleById(role.Id);
                var okayResult = result as OkObjectResult;
                Assert.NotNull(okayResult);
                Assert.Equal(200, okayResult.StatusCode);
                AspNetRole roleResult = okayResult.Value as AspNetRole;
                Assert.Equal("Production", roleResult.Name);
            }
        }
        [Fact]
        public void TestCreateRole()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                RoleController sut = new RoleController(roleRepository);
                AspNetRole role = new AspNetRole { Id = Guid.NewGuid().ToString(), Name = "Sales" };
                IActionResult result = sut.CreateRole(role);
                var createdResult = result as CreatedResult;
                Assert.NotNull(createdResult);
                Assert.Equal(201, createdResult.StatusCode);
                AspNetRole roleResult = createdResult.Value as AspNetRole;
                Assert.Equal("Sales", roleResult.Name);
                Assert.Equal("SALES", roleResult.NormalizedName);
                result = sut.CreateRole(null);
                var badResult = result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
                role = new AspNetRole { Id = Guid.NewGuid().ToString(), Name = string.Empty };
                result = sut.CreateRole(role);
                var bror = result as BadRequestObjectResult;
                Assert.Equal(400, bror.StatusCode);
                var modelState = sut.ModelState;
                Assert.Contains("Name", modelState.Keys);
                Assert.True(modelState["Name"].Errors.Count == 1);
                Assert.Equal("The Role Name should not be empty!", modelState["Name"].Errors[0].ErrorMessage);
                Assert.False(modelState.IsValid);
            }
        }
        [Fact]
        public void TestUpdateRole()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                RoleController sut = new RoleController(roleRepository);
                AspNetRole role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                role.Name = "Dev";
                IActionResult result = sut.UpdateRole(role);
                var noContent = result as NoContentResult;
                Assert.NotNull(noContent);
                Assert.Equal(204, noContent.StatusCode);
                role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Dev");
                Assert.NotNull(role);
                result = sut.UpdateRole(null);
                var badResult = result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
                role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Dev");
                role.Name = string.Empty;
                result = sut.UpdateRole(role);
                var bror = result as BadRequestObjectResult;
                Assert.Equal(400, bror.StatusCode);
                var modelState = sut.ModelState;
                Assert.Contains("Name", modelState.Keys);
                Assert.True(modelState["Name"].Errors.Count == 1);
                Assert.Equal("The Role Name should not be empty!", modelState["Name"].Errors[0].ErrorMessage);
                Assert.False(modelState.IsValid);
            }
        }
        [Fact]
        public void TestDeleteRole()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                RoleController sut = new RoleController(roleRepository);
                AspNetRole role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Production");
                IActionResult result = sut.DeleteRole(role.Id);
                var noContent = result as NoContentResult;
                Assert.NotNull(noContent);
                Assert.Equal(204, noContent.StatusCode);
            }
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                RoleController sut = new RoleController(roleRepository);
                var result = sut.DeleteRole(null);
                var badResult = result as BadRequestResult;
                Assert.Equal(400, badResult.StatusCode);
                result = sut.DeleteRole("BadData");
                var notFound = result as NotFoundResult;
                Assert.Equal(404, notFound.StatusCode);

            }
        }
    }
}
