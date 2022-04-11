using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAPI.Migrations
{
    public partial class dbChanges6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Observed");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Market",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Observed",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Observed");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Market",
                newName: "Stock");

            migrationBuilder.AddColumn<string>(
                name: "Stock",
                table: "Observed",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
