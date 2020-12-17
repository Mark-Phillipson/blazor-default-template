using System;
using System.Collections.Generic;
using MSPApplication.Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MSPApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<HRTask> Tasks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<CompanyDetail> CompanyDetails { get; set; }

        // ASP Identity Entities
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Views_Assigned_Role> Views_Assigned_Roles { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 1, Name = "Belgium" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 2, Name = "Germany" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 3, Name = "Netherlands" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 4, Name = "USA" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 5, Name = "Japan" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 6, Name = "China" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 7, Name = "UK" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 8, Name = "France" });
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 9, Name = "Brazil" });

            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 1, JobCategoryName = "Production" });
            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 2, JobCategoryName = "Sales" });
            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 3, JobCategoryName = "Management" });
            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 4, JobCategoryName = "Store staff" });
            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 5, JobCategoryName = "Finance" });
            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 6, JobCategoryName = "QA" });
            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 7, JobCategoryName = "IT" });
            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 8, JobCategoryName = "Cleaning" });
            modelBuilder.Entity<JobCategory>().HasData(new JobCategory() { JobCategoryId = 9, JobCategoryName = "Marketing" });

            modelBuilder.Entity<Employee>().HasData(new List<Employee>()
            {
                new Employee()
                {
                    EmployeeId = 1,
                    CountryId = 1,
                    MaritalStatus = MaritalStatus.Single,
                    BirthDate = new DateTime(1979, 1, 16),
                    City = "Maidstone",
                    Email = "MPhillipson0@Gmail.com",
                    FirstName = "Mark",
                    LastName = "Phillipson",
                    Gender = Gender.Male,
                    PhoneNumber = "324777888773",
                    Smoker = false,
                    Street = "Grote Markt 1",
                    Zip = "1000",
                    JobCategoryId = 1,
                    Comment = "Lorem Ipsum",
                    ExitDate = null,
                    JoinedDate = new DateTime(2015, 3, 1),
                    Latitude = 50.8503,
                    Longitude = 4.3517
                },
                new Employee()
                {
                    EmployeeId = 2,
                    CountryId = 1,
                    MaritalStatus = MaritalStatus.Single,
                    BirthDate = new DateTime(1979, 1, 16),
                    City = "New York",
                    Email = "testuser@domain.co.uk",
                    FirstName = "Test",
                    LastName = "User",
                    Gender = Gender.Female,
                    PhoneNumber = "55512312321",
                    Smoker = false,
                    Street = "Apple Road",
                    Zip = "59555",
                    JobCategoryId = 1,
                    Comment = "Lorem Ipsum",
                    ExitDate = null,
                    JoinedDate = new DateTime(2015, 3, 1),
                    Latitude = 46.8503,
                    Longitude = 48.3517
                }
            });

            modelBuilder.Entity<Currency>().HasData(new Currency()
            {
                Country = "USA",
                CurrencyId = 1,
                Name = "US Dollars",
                USExchange = 1
            });

            modelBuilder.Entity<Currency>().HasData(new Currency()
            {
                Country = "Germany",
                CurrencyId = 2,
                Name = "Euros",
                USExchange = 1.14
            });
            modelBuilder.Entity<Currency>().HasData(new Currency()
            {
                Country = "UK",
                CurrencyId = 3,
                Name = "Pounds",
                USExchange = 0.79
            });

            modelBuilder.Entity<Expense>().HasData(new Expense()
            {
                ExpenseId = 1,
                Title = "Conference Expense",
                Description = "I went to a conference",
                Amount = 900,
                ExpenseType = ExpenseType.Conference,
                Date = DateTime.Now,
                CurrencyId = 1,
                EmployeeId = 1
            });
            modelBuilder.Entity<HRTask>().HasData(new List<HRTask>()
            {
                new HRTask()
                {
                    HRTaskId = 1,
                    Description = "Peter is having an issue with his account login, please look into it.",
                    Title = "Employee New Start",
                    Status = HRTaskStatus.Open,
                    EmployeeId=1
                },
                new HRTask()
                {
                    HRTaskId = 2,
                    Description = "The fridge needs to be cleaned out and people are ignoring the weekly rotation.",
                    Title = "Kitchen Duty",
                    Status = HRTaskStatus.Open,
                    EmployeeId=1
                },
                new HRTask()
                {
                    HRTaskId = 3,
                    Description = "Schedule a welcome lunch for our new employees",
                    Title = "Welcome Lunch",
                    Status = HRTaskStatus.Open,
                    EmployeeId=1
                },
                new HRTask()
                {
                    HRTaskId = 4,
                    Description = "We need to purchase a new IT system for the management of the business.",
                    Title = "IT System",
                    Status = HRTaskStatus.Open,
                    EmployeeId=2
                }
            });
            // ASP Identity Entities
            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasDatabaseName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                //entity.Property(e => e.Ipaddress).HasColumnName("IPAddress");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasDatabaseName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasDatabaseName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Views_Assigned_Role>()
                .HasKey(c => new { c.Userid, c.RoleId });

        }
    }
}
