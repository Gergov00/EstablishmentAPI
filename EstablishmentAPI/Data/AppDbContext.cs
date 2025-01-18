using Microsoft.EntityFrameworkCore;
using EstablishmentAPI.Models;

namespace EstablishmentAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Establishment> Establishments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EstablishmentTag> EstablishmentTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи многие-ко-многим между Establishment и Tag
            modelBuilder.Entity<EstablishmentTag>()
                .HasKey(et => new { et.EstablishmentId, et.TagId });

            modelBuilder.Entity<EstablishmentTag>()
                .HasOne(et => et.Establishment)
                .WithMany(e => e.EstablishmentTags)
                .HasForeignKey(et => et.EstablishmentId);

            modelBuilder.Entity<EstablishmentTag>()
                .HasOne(et => et.Tag)
                .WithMany(t => t.EstablishmentTags)
                .HasForeignKey(et => et.TagId);
        }
    }
}
