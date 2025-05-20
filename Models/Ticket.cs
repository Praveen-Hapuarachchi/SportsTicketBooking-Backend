using System;
using System.ComponentModel.DataAnnotations;

namespace SportsTicketBooking.Models
{
    // This class represents a "Ticket" entity.
    // Each instance of this class corresponds to a ticket for a sports match.
    public class Ticket
    {
        // Primary key (automatically incremented by the database)
        public int Id { get; set; }

        // Name of the match (e.g., "Team A vs Team B")
        [Required] // This field must be provided; it's a required input
        public string MatchName { get; set; }

        // Description of the match (e.g., venue, teams, etc.)
        [Required]
        public string MatchDescription { get; set; }

        // Date and time of the match
        [Required]
        public DateTime MatchDate { get; set; }

        // URL or path to the match image (poster or banner)
        [Required]
        public string MatchImageUrl { get; set; }

        // Number of tickets available for this match
        [Required]
        public int TicketCount { get; set; }

        // ID of the admin who added this ticket
        // This helps track which admin created the ticket
        [Required]
        public int AdminId { get; set; }

        // Constructor to initialize the Ticket object with necessary values
        public Ticket(string matchName, string matchDescription, DateTime matchDate, string matchImageUrl, int ticketCount, int adminId)
        {
            MatchName = matchName;
            MatchDescription = matchDescription;
            MatchDate = matchDate;
            MatchImageUrl = matchImageUrl;
            TicketCount = ticketCount;
            AdminId = adminId;
        }
    }
}
