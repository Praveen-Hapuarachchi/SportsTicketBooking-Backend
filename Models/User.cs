using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsTicketBooking.Models
{
    // This class defines a User in the ticket booking system.
    // A User can be an Admin or a Customer (based on the UserType).
    public class User
    {
        // Primary key for the User table
        public int Id { get; set; }

        // User's first name (Required and limited to 50 characters)
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        // User's last name (Required and limited to 50 characters)
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        // User's email address (Required and must be a valid email format)
        [Required, EmailAddress]
        public string Email { get; set; }

        // Encrypted password (Required)
        [Required]
        public string PasswordHash { get; set; }

        // Role of the user (Admin or Customer) - Enum
        [Required]
        public UserType UserType { get; set; }

        // List of bookings made by the user
        // This is a navigation property to the related bookings
        public List<Booking> Bookings { get; set; } = new List<Booking>();

        // Constructor to initialize a User with essential properties
        public User(string firstName, string lastName, string email, string passwordHash, UserType userType)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            UserType = userType;
        }
    }
}
