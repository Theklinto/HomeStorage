using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.DataAccess.ImageEntities;
using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.DataAccess.ProductEntities;
using HomeStorage.DataAccess.UserEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            ValueConverter<DateTime, DateTime> dateTimeConverter = new(
    v => v.ToUniversalTime(),
    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            ValueConverter<DateTime?, DateTime?> nullableDateTimeConverter = new(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                    continue;

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }

            base.OnModelCreating(builder);
        }
    }
}
