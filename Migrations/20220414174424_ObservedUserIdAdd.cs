using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAPI.Migrations
{
    public partial class ObservedUserIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Observed",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Observed_CreatedById",
                table: "Observed",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Observed_Users_CreatedById",
                table: "Observed",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observed_Users_CreatedById",
                table: "Observed");

            migrationBuilder.DropIndex(
                name: "IX_Observed_CreatedById",
                table: "Observed");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Observed");
        }
    }
}
