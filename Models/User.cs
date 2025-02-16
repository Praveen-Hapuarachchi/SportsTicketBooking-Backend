using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsTicketBooking.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserType UserType { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();

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
