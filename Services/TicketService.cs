using Microsoft.EntityFrameworkCore;
using SportsTicketBooking.Data;
using SportsTicketBooking.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SportsTicketBooking.Services
{
    public class TicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddTicket(string matchName, string matchDescription, DateTime matchDate, string matchImageUrl, int ticketCount, int adminId)
        {
            var ticket = new Ticket(matchName, matchDescription, matchDate, matchImageUrl, ticketCount, adminId);
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Ticket>> GetAllTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket?> GetTicketById(int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task<bool> BookTicket(int ticketId, int userId, int count)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (ticket == null || user == null || ticket.TicketCount < count)
                return false;

            ticket.TicketCount -= count;

            var booking = new Booking(ticketId, ticket.MatchName, userId, $"{user.FirstName} {user.LastName}", count);
            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Booking>> GetUserBookings(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> CancelBooking(int bookingId, int userId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);
            if (booking == null) return false;

            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == booking.TicketId);
            if (ticket == null) return false;

            // Restore ticket count
            ticket.TicketCount += booking.Count;

            // Remove the booking
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Ticket>> GetTicketsByAdminId(int adminId)
        {
            return await _context.Tickets
                .Where(t => t.AdminId == adminId)
                .ToListAsync();
        }
    }
}
