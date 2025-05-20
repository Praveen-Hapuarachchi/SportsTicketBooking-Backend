using BCrypt.Net; // For password hashing using BCrypt
using Microsoft.EntityFrameworkCore; // For interacting with the database asynchronously
using Microsoft.IdentityModel.Tokens; // For JWT token security
using SportsTicketBooking.Data; // Accessing the database context
using SportsTicketBooking.Models; // Accessing User model and related types
using System.IdentityModel.Tokens.Jwt; // For creating JWT tokens
using System.Security.Claims; // For creating claims inside the JWT token
using System.Text; // For encoding the JWT secret key

namespace SportsTicketBooking.Services
{
    // This service handles authentication: Register, Login, and JWT Token Generation
    public class AuthService
    {
        private readonly ApplicationDbContext _context; // Used to access the database (Users table)
        private readonly IConfiguration _config; // Used to access JWT settings from appsettings.json

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // This method registers a new user in the database
        public async Task<bool> RegisterUser(string firstName, string lastName, string email, string password, UserType userType)
        {
            // Check if the email is already registered
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return false;

            // Hash the password using BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new User object
            var user = new User(firstName, lastName, email, hashedPassword, userType);

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true; // Registration successful
        }

        // This method handles login. It returns a JWT token if successful, or null if failed
        public async Task<string?> LoginUser(string email, string password)
        {
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            // If user not found or password does not match, return null
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            // Generate and return JWT token
            return GenerateJwtToken(user);
        }

        // This method retrieves a user by their email
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new InvalidOperationException("User not found."); // Error if email doesn't exist
            return user;
        }

        // Private helper method to generate a JWT token for a logged-in user
        private string GenerateJwtToken(User user)
        {
            // Get the secret key from appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.")
            ));

            // Create signing credentials using the key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Add claims to the token (user ID, email, role)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            };

            // Create the token with issuer, audience, claims, expiry, and credentials
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(
                    _config["Jwt:ExpireMinutes"] ?? throw new InvalidOperationException("Jwt:ExpireMinutes is not configured.")
                )),
                signingCredentials: creds
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
