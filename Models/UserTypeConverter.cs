using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SportsTicketBooking.Models
{
    public class UserTypeConverter : JsonConverter<UserType>
    {
        public override UserType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "Admin" => UserType.Admin,
                "User" => UserType.User,
                _ => throw new JsonException($"Invalid value for UserType: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, UserType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
