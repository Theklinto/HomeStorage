using HomeStorage.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.DbContext
{
    public class HomeStorageDbContext : IdentityDbContext<IdentityUser>
    {
        public HomeStorageDbContext(DbContextOptions<HomeStorageDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationUser> LocationUsers { get; set; }
        public DbSet<Image> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
