using AirCnC.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace AirCnC.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
