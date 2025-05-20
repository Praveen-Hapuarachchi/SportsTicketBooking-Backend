using Microsoft.EntityFrameworkCore;
using SportsTicketBooking.Models;

namespace SportsTicketBooking.Data
{
    // This class represents the EF Core database context.
    // It manages entity objects during run time, including database operations like querying and saving data.
    public class ApplicationDbContext : DbContext
    {
        // Constructor that accepts options to configure the context (like connection string, DB provider, etc.)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet properties represent tables in the database
        public DbSet<User> Users { get; set; }       // Table: Users
        public DbSet<Ticket> Tickets { get; set; }   // Table: Tickets
        public DbSet<Booking> Bookings { get; set; } // Table: Bookings

        // This method is used to configure the model (table structure, relationships, keys, etc.)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration for the User table
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); // Set 'Id' as the primary key
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(e => e.Id).HasColumnType("int"); // Store as int in the database

                // You can add other User property configurations here, e.g., required fields, max lengths, etc.
            });

            // Configuration for the Ticket table
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(e => e.MatchDate).HasColumnType("datetime"); // Store MatchDate as datetime in SQL
            });

            // Configuration for the Booking table
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key

                // Define the relationship: One User can have many Bookings
                entity.HasOne(b => b.User)         // Booking has one User
                      .WithMany(u => u.Bookings)   // User has many Bookings
                      .HasForeignKey(b => b.UserId); // Foreign key in Booking table
            });

            // Additional configurations (optional) could go here
        }
    }
}
