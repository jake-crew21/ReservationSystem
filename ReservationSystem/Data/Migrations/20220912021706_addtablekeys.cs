using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class addtablekeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sitting_Table_TableId",
                table: "Sitting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Table",
                table: "Table");

            migrationBuilder.AlterColumn<int>(
                name: "Area",
                table: "Table",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Table",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TableArea",
                table: "Sitting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TableId1",
                table: "Sitting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Table",
                table: "Table",
                columns: new[] { "Id", "Area" });

            migrationBuilder.CreateIndex(
                name: "IX_Sitting_TableId1_TableArea",
                table: "Sitting",
                columns: new[] { "TableId1", "TableArea" });

            migrationBuilder.AddForeignKey(
                name: "FK_Sitting_Table_TableId1_TableArea",
                table: "Sitting",
                columns: new[] { "TableId1", "TableArea" },
                principalTable: "Table",
                principalColumns: new[] { "Id", "Area" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sitting_Table_TableId1_TableArea",
                table: "Sitting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Table",
                table: "Table");

            migrationBuilder.DropIndex(
                name: "IX_Sitting_TableId1_TableArea",
                table: "Sitting");

            migrationBuilder.DropColumn(
                name: "TableArea",
                table: "Sitting");

            migrationBuilder.DropColumn(
                name: "TableId1",
                table: "Sitting");

            migrationBuilder.AlterColumn<int>(
                name: "Area",
                table: "Table",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Table",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Table",
                table: "Table",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sitting_Table_TableId",
                table: "Sitting",
                column: "TableId",
                principalTable: "Table",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
