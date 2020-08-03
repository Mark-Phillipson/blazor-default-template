using Microsoft.EntityFrameworkCore;
using MSPApplication.Data;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTestProject.Repositories
{
    public class UnitTestRoleRepository
    {
        protected DbContextOptions<AppDbContext> ContextOptions { get; set; }

        public UnitTestRoleRepository()
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
                var role1 = new AspNetRole { Id = Guid.NewGuid().ToString(), Name = "Production" };
                var role2 = new AspNetRole { Id = Guid.NewGuid().ToString(), Name = "Administration" };
                var role3 = new AspNetRole { Id = Guid.NewGuid().ToString(), Name = "Development" };
                context.AspNetRoles.AddRange(role1, role2, role3);
                context.SaveChanges();
            }
        }
        [Fact]
        public void DeleteRole_TestDeleteOkay()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                var role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                Assert.True(context.Set<AspNetRole>().Any(e => e.Id == role.Id));
                roleRepository.DeleteRole(role.Id);
                Assert.False(context.Set<AspNetRole>().Any(e => e.Id == role.Id));
            }
        }
        [Fact]
        public void GetAllRoles_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                List<AspNetRole> result = roleRepository.GetAllRoles().ToList();
                Assert.Equal(3, result.Count());
                Assert.Equal("Production", result[0].Name);
            }
        }
        [Fact]
        public void GetRoleById_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                var result = roleRepository.GetRoleById("BadData");
                Assert.Null(result);
                var role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Production");
                result = roleRepository.GetRoleById(role.Id);
                Assert.NotNull(result);
                Assert.Equal("Production", result.Name);
            }
        }
        [Fact]
        public void AddRole_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                var role = new AspNetRole { Name = "Testing", Id = Guid.NewGuid().ToString(), NormalizedName = "TESTING" };
                var result = roleRepository.AddRole(role);
                var savedResult = context.AspNetRoles.FirstOrDefault(e => e.Id == role.Id);
                Assert.Equal("Testing", savedResult.Name);
                Exception exception = Assert.Throws<ArgumentException>(() => result = roleRepository.AddRole(role));
                Assert.Contains("An item with the same key has already been added.", exception.Message);
                role = new AspNetRole { Name = "Testing", NormalizedName = "TESTING" };
                exception = Assert.Throws<ArgumentException>(() => result = roleRepository.AddRole(role));
            }
        }
        [Fact]
        public void UpdateRole_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                var role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                role.Name = "Testing";
                var result1 = roleRepository.UpdateRole(role);
                result1 = roleRepository.GetRoleById(role.Id);
                Assert.Equal("Testing", result1.Name);
            }
        }
        [Fact]
        public void DeleteRole_Test()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                RoleRepository roleRepository = new RoleRepository(context);
                roleRepository.DeleteRole("bad data");
                var role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                roleRepository.DeleteRole(role.Id);
                role = context.AspNetRoles.FirstOrDefault(e => e.Name == "Development");
                Assert.Null(role);
            }
        }
    }
}