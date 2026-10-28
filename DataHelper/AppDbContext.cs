
using EFCOREPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCOREPractice.DataHelper
{
    public class AppDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=.\SQLEXPRESS;Database=EFCoreDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .Property(d => d.DepartmentName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Department>()
                .Property(d => d.Location)
                .HasMaxLength(100);
        }
    }
}
