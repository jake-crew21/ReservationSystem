using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class StatusEnumAndResScaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_BookingStatus_BookingStatusStatus",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Session_SessionType",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Sitting_TableStatus_TableStatusStatus",
                table: "Sitting");

            migrationBuilder.DropIndex(
                name: "IX_Sitting_TableStatusStatus",
                table: "Sitting");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_BookingStatusStatus",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_SessionType",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "TableStatusStatus",
                table: "Sitting");

            migrationBuilder.DropColumn(
                name: "BookingStatusStatus",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservation");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Sitting",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "SessionType",
                table: "Reservation",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "BookingStatus",
                table: "Reservation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoOfTable",
                table: "Reservation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingStatus",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "NoOfTable",
                table: "Reservation");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Sitting",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TableStatusStatus",
                table: "Sitting",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SessionType",
                table: "Reservation",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BookingStatusStatus",
                table: "Reservation",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Sitting_TableStatusStatus",
                table: "Sitting",
                column: "TableStatusStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_BookingStatusStatus",
                table: "Reservation",
                column: "BookingStatusStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_SessionType",
                table: "Reservation",
                column: "SessionType");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_BookingStatus_BookingStatusStatus",
                table: "Reservation",
                column: "BookingStatusStatus",
                principalTable: "BookingStatus",
                principalColumn: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Session_SessionType",
                table: "Reservation",
                column: "SessionType",
                principalTable: "Session",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sitting_TableStatus_TableStatusStatus",
                table: "Sitting",
                column: "TableStatusStatus",
                principalTable: "TableStatus",
                principalColumn: "Status",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
