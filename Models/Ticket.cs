using System;
using System.ComponentModel.DataAnnotations;

namespace SportsTicketBooking.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public string MatchName { get; set; }

        [Required]
        public string MatchDescription { get; set; }

        [Required]
        public DateTime MatchDate { get; set; }

        [Required]
        public string MatchImageUrl { get; set; }

        [Required]
        public int TicketCount { get; set; }

        [Required]
        public int AdminId { get; set; } // Add this field to track the Admin who added the ticket

        public Ticket(string matchName, string matchDescription, DateTime matchDate, string matchImageUrl, int ticketCount, int adminId)
        {
            MatchName = matchName;
            MatchDescription = matchDescription;
            MatchDate = matchDate;
            MatchImageUrl = matchImageUrl;
            TicketCount = ticketCount;
            AdminId = adminId; // Initialize AdminId
        }
    }
}
