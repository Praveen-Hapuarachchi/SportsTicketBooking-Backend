using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsTicketBooking.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SportsTicketBooking.Controllers
{
    // Define the route for all endpoints in this controller and specify it as an API controller
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        // Constructor injection of TicketService, which handles business logic related to tickets
        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Endpoint to add a new ticket, accessible only to users with Admin role
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTicket([FromBody] AddTicketRequest request)
        {
            // Retrieve the user's ID from the JWT token claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                // If no user ID found in claims, respond with Unauthorized
                return Unauthorized("Invalid user.");
            }

            // Parse the user ID (admin ID) from the claim
            var adminId = int.Parse(userIdClaim.Value);

            // Call the ticket service to add the ticket
            var success = await _ticketService.AddTicket(
                request.MatchName, 
                request.MatchDescription, 
                request.MatchDate, 
                request.MatchImageUrl, 
                request.TicketCount, 
                adminId);

            // Return 500 if adding ticket fails
            if (!success)
                return StatusCode(500, "Error adding ticket");

            // Return success message
            return Ok("Ticket added successfully");
        }

        // Endpoint to get all tickets, accessible by any user (no authorization required)
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTickets();
            return Ok(tickets);
        }

        // Endpoint to book tickets for a specific ticket ID
        [HttpPost("book/{ticketId}")]
        public async Task<IActionResult> BookTicket(int ticketId, [FromBody] BookTicketRequest request)
        {
            // Attempt to book the requested number of tickets for the user
            var success = await _ticketService.BookTicket(ticketId, request.UserId, request.Count);

            // If booking fails due to insufficient tickets or invalid user, respond with BadRequest
            if (!success)
                return BadRequest("Booking failed, insufficient tickets or invalid user.");

            // Otherwise, respond with success message
            return Ok("Booking successful");
        }

        // Endpoint to get all bookings made by the logged-in user
        [HttpGet("mybookings")]
        [Authorize]  // User must be logged in (any role)
        public async Task<IActionResult> GetUserBookings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid user.");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Retrieve bookings for the current user from the service
            var bookings = await _ticketService.GetUserBookings(userId);

            // If no bookings found, return 404 Not Found
            if (bookings == null || bookings.Count == 0)
            {
                return NotFound("No bookings found for this user.");
            }

            // Return list of bookings
            return Ok(bookings);
        }

        // Endpoint to cancel a booking by booking ID for the logged-in user
        [HttpDelete("cancel/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid user.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID format.");
            }

            // Attempt to cancel the booking; only allow if booking belongs to the user
            var success = await _ticketService.CancelBooking(bookingId, userId);

            if (!success)
                return NotFound("Cancellation failed. Booking not found or unauthorized.");

            return Ok("Booking cancelled successfully.");
        }

        // Endpoint for Admins to get all tickets they have added
        [HttpGet("my-tickets")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid user.");
            }

            var adminId = int.Parse(userIdClaim.Value);

            // Get tickets created by the current admin
            var tickets = await _ticketService.GetTicketsByAdminId(adminId);
            return Ok(tickets);
        }

        // Endpoint for Admins to get all bookings for a specific ticket they manage
        [HttpGet("bookings/{ticketId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookingsForTicket(int ticketId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Invalid user.");

            var adminId = int.Parse(userIdClaim.Value);

            // Get bookings for the ticket, but only if the admin owns this ticket
            var bookings = await _ticketService.GetBookingsForTicket(ticketId, adminId);

            // Return bookings or 404 if none found
            return bookings.Count > 0 ? Ok(bookings) : NotFound("No bookings found for this ticket.");
        }
    }

    // Model class to receive ticket creation data from API requests
    public class AddTicketRequest
    {
        public string MatchName { get; set; }
        public string MatchDescription { get; set; }
        public DateTime MatchDate { get; set; }
        public string MatchImageUrl { get; set; }
        public int TicketCount { get; set; }

        // Constructor to initialize the AddTicketRequest object
        public AddTicketRequest(string matchName, string matchDescription, DateTime matchDate, string matchImageUrl, int ticketCount)
        {
            MatchName = matchName;
            MatchDescription = matchDescription;
            MatchDate = matchDate;
            MatchImageUrl = matchImageUrl;
            TicketCount = ticketCount;
        }
    }

    // Model class to receive ticket booking requests
    public class BookTicketRequest
    {
        public int UserId { get; set; }  // User who wants to book tickets
        public int Count { get; set; }   // Number of tickets to book
    }
}
