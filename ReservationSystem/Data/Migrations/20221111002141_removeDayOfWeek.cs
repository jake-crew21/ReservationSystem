using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Migrations
{
    public partial class removeDayOfWeek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SittingSchedule",
                table: "SittingSchedule");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "SittingSchedule");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "SittingSchedule",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "SittingSchedule",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SittingSchedule",
                table: "SittingSchedule",
                column: "SessionType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SittingSchedule",
                table: "SittingSchedule");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "SittingSchedule",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "SittingSchedule",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "SittingSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SittingSchedule",
                table: "SittingSchedule",
                columns: new[] { "SessionType", "StartDate" });
        }
    }
}
