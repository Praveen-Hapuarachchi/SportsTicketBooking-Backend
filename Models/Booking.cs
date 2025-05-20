using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsTicketBooking.Models
{
    // Represents a booking made by a user for a specific ticket/event
    public class Booking
    {
        // Primary key for the Booking entity
        public int Id { get; set; }

        // The ID of the ticket that has been booked (foreign key)
        [Required] 
        public int TicketId { get; set; }

        // The name/title of the ticket (e.g., "Cricket Final Match")
        [Required] 
        public string TicketName { get; set; }

        // The ID of the user who made the booking (foreign key)
        [Required] 
        public int UserId { get; set; }

        // The name of the user who made the booking
        [Required] 
        public string UserName { get; set; }

        // Number of tickets the user booked
        [Required] 
        public int Count { get; set; }

        // Navigation property to access related User entity details
        // This is optional (nullable) and helps with Entity Framework relationships
        public User? User { get; set; }

        // Constructor to initialize Booking with required details
        public Booking(int ticketId, string ticketName, int userId, string userName, int count)
        {
            TicketId = ticketId;
            TicketName = ticketName;
            UserId = userId;
            UserName = userName;
            Count = count;
        }
    }
}
