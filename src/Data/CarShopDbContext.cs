using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<CarImage> CarImages => Set<CarImage>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        }
    }
}
