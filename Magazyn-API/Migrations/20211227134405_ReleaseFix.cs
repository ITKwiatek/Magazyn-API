using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class ReleaseFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Releases_OrderId",
                table: "Releases");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_OrderId",
                table: "Releases",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Releases_OrderId",
                table: "Releases");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_OrderId",
                table: "Releases",
                column: "OrderId",
                unique: true);
        }
    }
}
