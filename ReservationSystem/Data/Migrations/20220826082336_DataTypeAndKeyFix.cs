using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class DataTypeAndKeyFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatingAreaArea = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Table_SeatingArea_SeatingAreaArea",
                        column: x => x.SeatingAreaArea,
                        principalTable: "SeatingArea",
                        principalColumn: "Area");
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Contact = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResDate = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfPpl = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingStatusStatus = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SessionType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatingAreaArea = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => new { x.Contact, x.ResDate, x.StartTime });
                    table.ForeignKey(
                        name: "FK_Reservation_BookingStatus_BookingStatusStatus",
                        column: x => x.BookingStatusStatus,
                        principalTable: "BookingStatus",
                        principalColumn: "Status");
                    table.ForeignKey(
                        name: "FK_Reservation_SeatingArea_SeatingAreaArea",
                        column: x => x.SeatingAreaArea,
                        principalTable: "SeatingArea",
                        principalColumn: "Area",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Session_SessionType",
                        column: x => x.SessionType,
                        principalTable: "Session",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sitting",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResDate = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableStatusStatus = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sitting", x => new { x.TableId, x.Contact, x.ResDate, x.StartTime });
                    table.ForeignKey(
                        name: "FK_Sitting_Reservation_Contact_ResDate_StartTime",
                        columns: x => new { x.Contact, x.ResDate, x.StartTime },
                        principalTable: "Reservation",
                        principalColumns: new[] { "Contact", "ResDate", "StartTime" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sitting_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sitting_TableStatus_TableStatusStatus",
                        column: x => x.TableStatusStatus,
                        principalTable: "TableStatus",
                        principalColumn: "Status",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_BookingStatusStatus",
                table: "Reservation",
                column: "BookingStatusStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_SeatingAreaArea",
                table: "Reservation",
                column: "SeatingAreaArea");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_SessionType",
                table: "Reservation",
                column: "SessionType");

            migrationBuilder.CreateIndex(
                name: "IX_Sitting_Contact_ResDate_StartTime",
                table: "Sitting",
                columns: new[] { "Contact", "ResDate", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Sitting_TableStatusStatus",
                table: "Sitting",
                column: "TableStatusStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Table_SeatingAreaArea",
                table: "Table",
                column: "SeatingAreaArea");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sitting");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "TableStatus");

            migrationBuilder.DropTable(
                name: "BookingStatus");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "SeatingArea");
        }
    }
}
