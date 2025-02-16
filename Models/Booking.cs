using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsTicketBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required]
        public string TicketName { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public int Count { get; set; }

        public User? User { get; set; }  // Nullable navigation property

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
