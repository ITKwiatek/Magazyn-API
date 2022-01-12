using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class issuer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssuerId",
                table: "Releases",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Releases_IssuerId",
                table: "Releases",
                column: "IssuerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_AspNetUsers_IssuerId",
                table: "Releases",
                column: "IssuerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Releases_AspNetUsers_IssuerId",
                table: "Releases");

            migrationBuilder.DropIndex(
                name: "IX_Releases_IssuerId",
                table: "Releases");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Releases");
        }
    }
}
