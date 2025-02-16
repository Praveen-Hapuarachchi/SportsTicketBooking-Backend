using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminIdToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Tickets");
        }
    }
}
