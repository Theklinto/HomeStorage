using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.API.DbContext
{
    public class HomeStorageDbContext : IdentityDbContext<IdentityUser>
    {
        public HomeStorageDbContext(DbContextOptions<HomeStorageDbContext> dbContextOptions) : base(dbContextOptions) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
