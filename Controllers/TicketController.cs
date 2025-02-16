using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsTicketBooking.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SportsTicketBooking.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTicket([FromBody] AddTicketRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid user.");
            }

            var adminId = int.Parse(userIdClaim.Value);
            var success = await _ticketService.AddTicket(request.MatchName, request.MatchDescription, request.MatchDate, request.MatchImageUrl, request.TicketCount, adminId);
            if (!success)
                return StatusCode(500, "Error adding ticket");

            return Ok("Ticket added successfully");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTickets();
            return Ok(tickets);
        }

        [HttpPost("book/{ticketId}")]
        public async Task<IActionResult> BookTicket(int ticketId, [FromBody] BookTicketRequest request)
        {
            var success = await _ticketService.BookTicket(ticketId, request.UserId, request.Count);
            if (!success)
                return BadRequest("Booking failed, insufficient tickets or invalid user.");

            return Ok("Booking successful");
        }

        // New endpoint to retrieve user bookings
        [HttpGet("mybookings")]
        [Authorize]
        public async Task<IActionResult> GetUserBookings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid user.");
            }

            int userId = int.Parse(userIdClaim.Value);
            var bookings = await _ticketService.GetUserBookings(userId);

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound("No bookings found for this user.");
            }

            return Ok(bookings);
        }

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

            var success = await _ticketService.CancelBooking(bookingId, userId);

            if (!success)
                return NotFound("Cancellation failed. Booking not found or unauthorized.");

            return Ok("Booking cancelled successfully.");
        }

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
            var tickets = await _ticketService.GetTicketsByAdminId(adminId);
            return Ok(tickets);
        }
    }

    public class AddTicketRequest
    {
        public string MatchName { get; set; }
        public string MatchDescription { get; set; }
        public DateTime MatchDate { get; set; }
        public string MatchImageUrl { get; set; }
        public int TicketCount { get; set; }

        public AddTicketRequest(string matchName, string matchDescription, DateTime matchDate, string matchImageUrl, int ticketCount)
        {
            MatchName = matchName;
            MatchDescription = matchDescription;
            MatchDate = matchDate;
            MatchImageUrl = matchImageUrl;
            TicketCount = ticketCount;
        }
    }

    public class BookTicketRequest
    {
        public int UserId { get; set; }
        public int Count { get; set; }
    }
}
