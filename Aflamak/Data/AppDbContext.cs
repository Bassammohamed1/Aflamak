using Aflamak.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aflamak.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<TvShow> TvShows { get; set; }
        public DbSet<ActorFilms> ActorFilms { get; set; }
        public DbSet<ActorTvShows> ActorTvShows { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ActorFilms>().HasKey(k => new { k.FilmId, k.ActorId });
            modelBuilder.Entity<ActorFilms>().HasOne(o => o.Actor).WithMany(m => m.ActorFilms).HasForeignKey(o => o.ActorId);
            modelBuilder.Entity<ActorFilms>().HasOne(o => o.Film).WithMany(m => m.ActorFilms).HasForeignKey(o => o.FilmId);

            modelBuilder.Entity<ActorTvShows>().HasKey(k => new { k.TvShowId, k.ActorId });
            modelBuilder.Entity<ActorTvShows>().HasOne(o => o.Actor).WithMany(m => m.ActorTvShows).HasForeignKey(o => o.ActorId);
            modelBuilder.Entity<ActorTvShows>().HasOne(o => o.TvShow).WithMany(m => m.ActorTvShows).HasForeignKey(o => o.TvShowId);
        }
    }
}
