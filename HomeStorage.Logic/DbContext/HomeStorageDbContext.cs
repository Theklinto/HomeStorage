using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.DataAccess.ImageEntities;
using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.DataAccess.ProductEntities;
using HomeStorage.DataAccess.UserEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.DbContext
{
    public class HomeStorageDbContext(DbContextOptions<HomeStorageDbContext> dbContextOptions) : IdentityDbContext<HomeStorageUser, HomeStorageRole, Guid, HomeStorageUserClaim, HomeStorageUserRole, HomeStorageUserLogin, HomeStorageRoleClaim, HomeStorageUserToken>(dbContextOptions)
    {
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public DbSet<LocationUser> LocationUsers { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
