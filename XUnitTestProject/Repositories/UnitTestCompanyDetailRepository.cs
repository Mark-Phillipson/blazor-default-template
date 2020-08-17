using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MSPApplication.Data;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace XUnitTestProject.Repositories
{
    public class UnitTestCompanyDetailRepository : IDisposable
    {
        protected DbContextOptions<AppDbContext> ContextOptions { get; set; }
        private readonly DbConnection _connection;
        public UnitTestCompanyDetailRepository()
        {
            ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            //.UseInMemoryDatabase(Guid.NewGuid().ToString())
            //.UseSqlite(CreateInMemoryDatabase())
            .UseSqlite($"Filename={Guid.NewGuid().ToString()}.db")
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
                var companyDetail = new CompanyDetail { Id = 1, Active = true, AddressLine1 = "1 The Road", City = "The City", CompanyName = "Company Name 1", CountryId = 1, EmailAddress = "weurxknovj@gmail.com", WebAddress = "test.co.uk" };
                context.CompanyDetails.Add(companyDetail);
                context.SaveChanges();
            }
        }
        [Fact]
        public void DeleteCompanyDetail_TestDeleteOkay()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                CompanyDetailRepository sut = new CompanyDetailRepository(context);
                var companyDetail = context.CompanyDetails.FirstOrDefault(e => e.CompanyName == "Company Name 1");
                Assert.True(context.Set<CompanyDetail>().Any(e => e.Id == companyDetail.Id));
                sut.DeleteCompanyDetail(companyDetail.Id);
                Assert.False(context.Set<CompanyDetail>().Any(e => e.Id == companyDetail.Id));
            }
        }
        [Fact]
        public void GetCompanyDetailByIdTest()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                CompanyDetailRepository companyDetailRepository = new CompanyDetailRepository(context);
                var companyDetail = context.CompanyDetails.FirstOrDefault(e => e.CompanyName == "Company Name 1");
                var result = companyDetailRepository.GetCompanyDetailById(companyDetail.Id);
                Assert.Equal(companyDetail.Id, result.Id);
                result = companyDetailRepository.GetCompanyDetailById(0);
                Assert.Null(result);
            }
        }
        [Fact]
        public void GetAllCompanyDetails_Test()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                CompanyDetailRepository companyDetailRepository = new CompanyDetailRepository(context);
                List<CompanyDetail> result = companyDetailRepository.GetAllCompanyDetails().ToList();
                Assert.Single(result);
                Assert.Contains("Company Name 1", result[0].CompanyName);
            }
        }
        [Fact]
        public void AddCompanyDetail_Test()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                CompanyDetailRepository companyDetailRepository = new CompanyDetailRepository(context);
                var companyDetail = new CompanyDetail { Id = 2, Active = true, AddressLine1 = "2 The Road", City = "The City", CompanyName = "Company Name 2", CountryId = 1, EmailAddress = "ybtxuyroto@gmail.com", WebAddress = "test2.co.uk" };
                var result = companyDetailRepository.AddCompanyDetail(companyDetail);
                var savedResult = context.CompanyDetails.FirstOrDefault(e => e.Id == companyDetail.Id);
                Assert.Equal("Company Name 2", savedResult.CompanyName);
                Exception exception = Assert.Throws<DbUpdateException>(() => result = companyDetailRepository.AddCompanyDetail(companyDetail));
                Assert.Contains("See the inner exception for details", exception.Message);
            }
        }
        [Fact]
        public void UpdateCompanyDetail_Test()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                CompanyDetailRepository companyDetailRepository = new CompanyDetailRepository(context);
                var companyDetail = context.CompanyDetails.FirstOrDefault(e => e.CompanyName == "Company Name 1");
                companyDetail.CompanyName = "Company Name 3";
                var result1 = companyDetailRepository.UpdateCompanyDetail(companyDetail);
                result1 = companyDetailRepository.GetCompanyDetailById(companyDetail.Id);
                Assert.Equal("Company Name 3", result1.CompanyName);
            }
        }
        [Fact]
        public void DeleteCompanyDetail_Test()
        {
            SeedData();
            using (var context = new AppDbContext(ContextOptions))
            {
                CompanyDetailRepository companyDetailRepository = new CompanyDetailRepository(context);
                var companyDetail = context.CompanyDetails.FirstOrDefault(e => e.CompanyName == "Company Name 1");
                companyDetailRepository.DeleteCompanyDetail(companyDetail.Id);
                companyDetail = context.CompanyDetails.FirstOrDefault(e => e.CompanyName == "Company Name 1");
                Assert.Null(companyDetail);
            }
        }
        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
