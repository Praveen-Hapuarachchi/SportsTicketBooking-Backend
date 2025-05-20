using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SportsTicketBooking.Models
{
    // This custom JSON converter handles converting the UserType enum to/from JSON strings.
    // It's useful when sending or receiving UserType as a string (like "Admin" or "User") in APIs.

    public class UserTypeConverter : JsonConverter<UserType>
    {
        // This method is called when reading/deserializing JSON into a UserType object
        public override UserType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the string value from the JSON (e.g., "Admin" or "User")
            var value = reader.GetString();

            // Convert the string to the corresponding UserType enum value
            return value switch
            {
                "Admin" => UserType.Admin,
                "User" => UserType.User,
                _ => throw new JsonException($"Invalid value for UserType: {value}") // Throw an error if value is invalid
            };
        }

        // This method is called when writing/serializing the UserType enum into JSON
        public override void Write(Utf8JsonWriter writer, UserType value, JsonSerializerOptions options)
        {
            // Convert the UserType enum (e.g., UserType.Admin) to string and write it to JSON
            writer.WriteStringValue(value.ToString());
        }
    }
}
