using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation
{
    internal class MovieDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<ShowTime> ShowTimes { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-LM1CPN7\\SQLEXPRESS;Database=Movie_Reservation;" +
                "Trusted_Connection=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.user)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.userId);

            modelBuilder.Entity<ShowTime>()
                .HasOne(st => st.movie)
                .WithMany(m => m.ShowTimes)
                .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<ShowTime>()
                .HasMany(r => r.Seats)
                .WithOne(s => s.ShowTime)
                .HasForeignKey(r => r.ShowTimeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.RservedSeats)
                .WithOne(s => s.Reservation)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ShowTime)
                .WithMany(st => st.reservations)
                .HasForeignKey(r => r.showTimeId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
