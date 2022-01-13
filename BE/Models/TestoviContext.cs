using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class TestoviContext : DbContext
    {
        public DbSet<Set> Setovi { get; set; }
        public DbSet<Spojnica> Spojnice { get; set; }
        public DbSet<Tag> Tagovi { get; set; }
        public DbSet<Pitanje> Pitanja { get; set; }
        public DbSet<SpojnicePitanja> SpojnicePitanja { get; set; } // Spojevi
        public DbSet<SpojniceTagovi> SpojniceTagovi { get; set; } // Spojevi
        public DbSet<SetTagovi> SetTagovi { get; set; } // Spojevi
        public DbSet<SetSpojnice> SetSpojnice { get; set; } // Spojevi
        public DbSet<SetPitanja> SetPitanja { get; set; } // Spojevi

        public TestoviContext(DbContextOptions options) : base(options)
        {

        }

        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Predmet>()
                    .HasMany<Spoj>(p => p.PredmetStudent)
                    .WithOne(p => p.Predmet);
        } */
    }
}