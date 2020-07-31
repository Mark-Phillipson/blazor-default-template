using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MSPApplication.Data
{
    public class AppIdentityDbContext : IdentityDbContext
    {
        //public AppIdentityDbContext()
        //{

        //}
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }
    }
}
