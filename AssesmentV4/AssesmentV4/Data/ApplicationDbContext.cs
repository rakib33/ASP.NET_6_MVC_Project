using System;
using System.ComponentModel.DataAnnotations;
using AssesmentV4.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace AssesmentV4.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
            entity.ToTable("Products");
            entity.Property(p => p.OrderDate)
            .HasColumnType("timestamp without time zone");
            });
        }

    }
}
