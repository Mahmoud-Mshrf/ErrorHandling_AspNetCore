using ErrorHandling_AspNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ErrorHandling_AspNetCore.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Driver> Drivers{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
