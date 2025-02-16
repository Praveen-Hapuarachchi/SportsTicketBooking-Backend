using Microsoft.AspNetCore.Mvc;
using SportsTicketBooking.Models;
using SportsTicketBooking.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SportsTicketBooking.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var success = await _authService.RegisterUser(request.FirstName, request.LastName, request.Email, request.Password, request.UserType);
                if (!success)
                    return BadRequest("Email already exists");

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginUser(request.Email, request.Password);
            if (token == null)
                return Unauthorized("Invalid credentials");

            var user = await _authService.GetUserByEmail(request.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { token, user.FirstName, user.LastName, user.UserType, user.Id, user.Email });
        }
    }

    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        [EnumDataType(typeof(UserType))]
        [JsonConverter(typeof(UserTypeConverter))]
        public UserType UserType { get; set; }  // "User" or "Admin"

        public RegisterRequest(string firstName, string lastName, string email, string password, UserType userType)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserType = userType;
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}