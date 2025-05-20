namespace SportsTicketBooking.Models
{
    // This enum defines the types of users in the Sports Ticket Booking System.
    // An enum (short for "enumeration") is a special data type that allows you to define a set of named constants.

    public enum UserType
    {
        // Represents a regular user (i.e., a customer who books tickets)
        User,

        // Represents an administrator (i.e., a person who can add, edit, or delete tickets, and manage the system)
        Admin
    }
}
