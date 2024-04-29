using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        //protected readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieModel>().Property(m => m.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<PersonModel>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<GenreModel>().Property(g => g.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProfessionModel>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<MoviePersonModel>().HasKey( i => new {i.MovieId, i.ActorId, i.RoleId});

            //relationship between table 'Movie' and 'MoviePerson'
            modelBuilder.Entity<MoviePersonModel>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MoviePerson)
                .HasForeignKey(x => x.MovieId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //relationship between table 'Person' and 'MoviePerson'
            modelBuilder.Entity<MoviePersonModel>()
                .HasOne(x => x.Actor)
                .WithMany(y => y.MoviePerson)
                .HasForeignKey(f => f.ActorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //relationship between table 'Profession' and 'MoviePerson'
            modelBuilder.Entity<MoviePersonModel>()
                .HasOne(x => x.Role)
                .WithMany(y => y.MoviePerson)
                .HasForeignKey(f => f.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieGenreModel>().HasKey( i => new {i.MovieId, i.GenreId});

            //relationship between table 'Movie' and 'MovieGenre'
            modelBuilder.Entity<MovieGenreModel>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieGenre)
                .HasForeignKey(f => f.MovieId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //relationship between table 'Genre' and 'MovieGenre'
            modelBuilder.Entity<MovieGenreModel>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.MovieGenre)
                .HasForeignKey(f => f.GenreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }

        public DbSet<MovieModel> Movie => Set<MovieModel>();
        public DbSet<PersonModel> Person => Set<PersonModel>();
        public DbSet<ProfessionModel> Role => Set<ProfessionModel>();
        public DbSet<GenreModel> Genre => Set<GenreModel>();
        public DbSet<MoviePersonModel> MoviePerson => Set<MoviePersonModel>();
        public DbSet<MovieGenreModel> MovieGenre => Set<MovieGenreModel>();
    }
}
