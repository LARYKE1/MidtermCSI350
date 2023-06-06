using Microsoft.EntityFrameworkCore;
using MidtermCSI350.Models;

namespace MidtermCSI350.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Manager { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext>options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(e => e.ManagerId);

            // Additional configuration or entity mappings can be added here

            base.OnModelCreating(modelBuilder);
        }
    }
}
