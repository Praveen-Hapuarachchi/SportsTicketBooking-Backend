// Required namespaces
using Microsoft.AspNetCore.Mvc;
using SportsTicketBooking.Models;
using SportsTicketBooking.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SportsTicketBooking.Controllers
{
    // Route prefix for all actions in this controller will be 'api/auth'
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Dependency injection of AuthService to handle user registration and login
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/register
        // Registers a new user (Admin or User)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Call AuthService to register the user
                var success = await _authService.RegisterUser(
                    request.FirstName, 
                    request.LastName, 
                    request.Email, 
                    request.Password, 
                    request.UserType);

                // If email already exists, return 400 Bad Request
                if (!success)
                    return BadRequest("Email already exists");

                // If registration is successful
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                // If unexpected error occurs, return 500 Internal Server Error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/auth/login
        // Logs in an existing user and returns a JWT token with user info
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Call AuthService to validate credentials and generate token
            var token = await _authService.LoginUser(request.Email, request.Password);

            // If credentials are invalid, return 401 Unauthorized
            if (token == null)
                return Unauthorized("Invalid credentials");

            // Retrieve the user details by email
            var user = await _authService.GetUserByEmail(request.Email);

            // Double-check user exists
            if (user == null)
                return Unauthorized("Invalid credentials");

            // Return token and some user details
            return Ok(new 
            { 
                token, 
                user.FirstName, 
                user.LastName, 
                user.UserType, 
                user.Id, 
                user.Email 
            });
        }
    }

    // Request model for user registration
    public class RegisterRequest
    {
        // User's first name
        public string FirstName { get; set; }

        // User's last name
        public string LastName { get; set; }

        // Email used for login
        public string Email { get; set; }

        // Password for the account
        public string Password { get; set; }

        // UserType must be either 'User' or 'Admin'
        [Required]
        [EnumDataType(typeof(UserType))]  // Validates that the value is one of the enum values
        [JsonConverter(typeof(UserTypeConverter))]  // Allows enum to be converted from string in JSON
        public UserType UserType { get; set; }

        // Constructor to initialize all fields
        public RegisterRequest(string firstName, string lastName, string email, string password, UserType userType)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserType = userType;
        }
    }

    // Request model for user login
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        // Constructor to initialize login fields
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
