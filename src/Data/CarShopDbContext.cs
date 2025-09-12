using Microsoft.EntityFrameworkCore;

namespace CarShopApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Car> Cars => Set<Car>();
    }
}
