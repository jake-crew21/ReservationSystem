using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class setTbStatSessArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookingStatus",
                column: "Status",
                values: new object[]
                {
                    "Cancelled",
                    "Completed",
                    "Confirmed",
                    "Pending",
                    "Seated"
                });

            migrationBuilder.InsertData(
                table: "Session",
                column: "Type",
                values: new object[]
                {
                    "Breakfast",
                    "Dinner",
                    "Lunch"
                });

            migrationBuilder.InsertData(
                table: "TableStatus",
                column: "Status",
                values: new object[]
                {
                    "Closed",
                    "Occupied",
                    "Open",
                    "Reserved"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookingStatus",
                keyColumn: "Status",
                keyValue: "Cancelled");

            migrationBuilder.DeleteData(
                table: "BookingStatus",
                keyColumn: "Status",
                keyValue: "Completed");

            migrationBuilder.DeleteData(
                table: "BookingStatus",
                keyColumn: "Status",
                keyValue: "Confirmed");

            migrationBuilder.DeleteData(
                table: "BookingStatus",
                keyColumn: "Status",
                keyValue: "Pending");

            migrationBuilder.DeleteData(
                table: "BookingStatus",
                keyColumn: "Status",
                keyValue: "Seated");

            migrationBuilder.DeleteData(
                table: "Session",
                keyColumn: "Type",
                keyValue: "Breakfast");

            migrationBuilder.DeleteData(
                table: "Session",
                keyColumn: "Type",
                keyValue: "Dinner");

            migrationBuilder.DeleteData(
                table: "Session",
                keyColumn: "Type",
                keyValue: "Lunch");

            migrationBuilder.DeleteData(
                table: "TableStatus",
                keyColumn: "Status",
                keyValue: "Closed");

            migrationBuilder.DeleteData(
                table: "TableStatus",
                keyColumn: "Status",
                keyValue: "Occupied");

            migrationBuilder.DeleteData(
                table: "TableStatus",
                keyColumn: "Status",
                keyValue: "Open");

            migrationBuilder.DeleteData(
                table: "TableStatus",
                keyColumn: "Status",
                keyValue: "Reserved");
        }
    }
}
