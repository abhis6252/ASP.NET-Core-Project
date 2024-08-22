using Microsoft.EntityFrameworkCore;
using SongNewApi.Models;

namespace SongNewApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Song> Songs { get; set; }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Category> Categories { get; set; }

        public override Task<int> SaveChangesAsync( CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Base>()) 
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "system";
                }
                else if (entry.State == EntityState.Modified)
                { 
                    entry.Entity.ModifiedDate = DateTime.Now;
                    entry.Entity.ModifiedBy = "system";
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
