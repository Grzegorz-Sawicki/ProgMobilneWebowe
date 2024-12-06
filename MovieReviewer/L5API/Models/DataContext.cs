using Microsoft.EntityFrameworkCore;
using L5Shared.Models;

namespace L5API.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieDetails> MovieDetails { get; set; }
        public DbSet<MovieNote> MovieNotes { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Actor> Actors { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Details)
                .WithOne()
                .HasForeignKey<MovieDetails>(md => md.ID);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Note)
                .WithOne()
                .HasForeignKey<MovieNote>(mn => mn.ID);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorID);

            modelBuilder.Entity<Movie>()
             .HasMany(m => m.Actors)
             .WithMany(a => a.Movies)
             .UsingEntity<Dictionary<string, object>>(
                 "MovieActor",
                 j => j.HasOne<Actor>().WithMany().HasForeignKey("ActorID"),
                 j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieID"));
        }
    }
}
