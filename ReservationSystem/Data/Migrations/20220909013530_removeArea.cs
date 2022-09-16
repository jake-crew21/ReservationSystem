using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class removeArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_SeatingArea_SeatingAreaArea",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Table_SeatingArea_SeatingAreaArea",
                table: "Table");

            migrationBuilder.DropTable(
                name: "BookingStatus");

            migrationBuilder.DropTable(
                name: "SeatingArea");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "TableStatus");

            migrationBuilder.DropIndex(
                name: "IX_Table_SeatingAreaArea",
                table: "Table");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_SeatingAreaArea",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "SeatingAreaArea",
                table: "Table");

            migrationBuilder.DropColumn(
                name: "SeatingAreaArea",
                table: "Reservation");

            migrationBuilder.AlterColumn<int>(
                name: "Area",
                table: "Table",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Area",
                table: "Reservation",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "Table",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SeatingAreaArea",
                table: "Table",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SeatingAreaArea",
                table: "Reservation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BookingStatus",
                columns: table => new
                {
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingStatus", x => x.Status);
                });

            migrationBuilder.CreateTable(
                name: "SeatingArea",
                columns: table => new
                {
                    Area = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatingArea", x => x.Area);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "TableStatus",
                columns: table => new
                {
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableStatus", x => x.Status);
                });

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
                table: "SeatingArea",
                column: "Area",
                values: new object[]
                {
                    "Balcony",
                    "Main",
                    "Outside"
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

            migrationBuilder.CreateIndex(
                name: "IX_Table_SeatingAreaArea",
                table: "Table",
                column: "SeatingAreaArea");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_SeatingAreaArea",
                table: "Reservation",
                column: "SeatingAreaArea");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_SeatingArea_SeatingAreaArea",
                table: "Reservation",
                column: "SeatingAreaArea",
                principalTable: "SeatingArea",
                principalColumn: "Area",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Table_SeatingArea_SeatingAreaArea",
                table: "Table",
                column: "SeatingAreaArea",
                principalTable: "SeatingArea",
                principalColumn: "Area");
        }
    }
}
