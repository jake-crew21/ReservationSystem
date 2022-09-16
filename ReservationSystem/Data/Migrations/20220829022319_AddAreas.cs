using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class AddAreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SeatingArea",
                column: "Area",
                value: "Balcony");

            migrationBuilder.InsertData(
                table: "SeatingArea",
                column: "Area",
                value: "Main");

            migrationBuilder.InsertData(
                table: "SeatingArea",
                column: "Area",
                value: "Outside");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SeatingArea",
                keyColumn: "Area",
                keyValue: "Balcony");

            migrationBuilder.DeleteData(
                table: "SeatingArea",
                keyColumn: "Area",
                keyValue: "Main");

            migrationBuilder.DeleteData(
                table: "SeatingArea",
                keyColumn: "Area",
                keyValue: "Outside");
        }
    }
}
