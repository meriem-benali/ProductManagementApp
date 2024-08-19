#nullable disable
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data
{
    public class CleanArchitectureContext : DbContextBase , ICleanArchitectureContext
    {
        public CleanArchitectureContext(DbContextOptions<CleanArchitectureContext> options) : base(options) { }

        
        public DbSet<Product> Products { get; set; }


        /// <summary>
        /// On model creating
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanArchitectureContext).Assembly);
            
            modelBuilder.Entity<Product>().HasKey(t => new { t.Id});

        }

        /// <summary>
        /// Called before save changes.
        /// </summary>
        protected override void OnBeforeSaveChanges()
        {
            UseAuditable();
            UseSoftDelete();
            base.OnBeforeSaveChanges();
        }

    }
}
