using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class TestoviContext : DbContext
    {
        // public DbSet<Student> Studenti { get; set; }
        public DbSet<Spojnica> Spojnice { get; set; }
        // public DbSet<Tag> Predmeti { get; set; }
        public DbSet<Tag> Tagovi { get; set; }
        // public DbSet<IspitniRok> Rokovi { get; set; }
        public DbSet<Pitanje> Pitanja { get; set; }
        //public DbSet<Spoj> StudentiPredmeti { get; set; } // Spojevi

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