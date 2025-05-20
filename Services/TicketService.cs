using Microsoft.EntityFrameworkCore; // For working with Entity Framework Core
using SportsTicketBooking.Data; // Accessing the database context
using SportsTicketBooking.Models; // Using Ticket and Booking models
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SportsTicketBooking.Services
{
    // Service class to handle ticket-related operations
    public class TicketService
    {
        private readonly ApplicationDbContext _context; // Database context to interact with the database

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new ticket to the system (Admin creates a ticket for a match)
        public async Task<bool> AddTicket(string matchName, string matchDescription, DateTime matchDate, string matchImageUrl, int ticketCount, int adminId)
        {
            var ticket = new Ticket(matchName, matchDescription, matchDate, matchImageUrl, ticketCount, adminId);

            _context.Tickets.Add(ticket); // Add ticket to the DB
            await _context.SaveChangesAsync(); // Save changes
            return true;
        }

        // Get all available tickets (used to display tickets to users)
        public async Task<List<Ticket>> GetAllTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        // Get a single ticket by its ID
        public async Task<Ticket?> GetTicketById(int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        // Book a ticket for a user
        public async Task<bool> BookTicket(int ticketId, int userId, int count)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            // Validate ticket and user existence and ticket availability
            if (ticket == null || user == null || ticket.TicketCount < count)
                return false;

            // Reduce the ticket count
            ticket.TicketCount -= count;

            // Create a new booking record
            var booking = new Booking(ticketId, ticket.MatchName, userId, $"{user.FirstName} {user.LastName}", count);
            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync(); // Save the booking and ticket update
            return true;
        }

        // Get all bookings for a specific user (e.g., user's booking history)
        public async Task<List<Booking>> GetUserBookings(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        // Cancel a user's booking
        public async Task<bool> CancelBooking(int bookingId, int userId)
        {
            // Find the booking belonging to this user
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);
            if (booking == null) return false;

            // Find the associated ticket to restore count
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == booking.TicketId);
            if (ticket == null) return false;

            // Increase the ticket count back
            ticket.TicketCount += booking.Count;

            // Remove the booking from the database
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get all tickets created by a specific admin (for admin dashboard)
        public async Task<List<Ticket>> GetTicketsByAdminId(int adminId)
        {
            return await _context.Tickets
                .Where(t => t.AdminId == adminId)
                .ToListAsync();
        }

        // Get all users who booked a specific ticket (admin-level access)
        public async Task<List<object>> GetBookingsForTicket(int ticketId, int adminId)
        {
            // Find the ticket and make sure the requesting admin owns it
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            if (ticket == null || ticket.AdminId != adminId)
                return new List<object>(); // Ticket doesn't exist or not owned by this admin

            // Retrieve booking info (user names and counts)
            var bookings = await _context.Bookings
                .Where(b => b.TicketId == ticketId)
                .Select(b => new
                {
                    UserFullName = b.UserName,
                    TicketCount = b.Count
                })
                .ToListAsync();

            // Convert to object list (since anonymous types can't be returned directly)
            return bookings.Cast<object>().ToList();
        }
    }
}
