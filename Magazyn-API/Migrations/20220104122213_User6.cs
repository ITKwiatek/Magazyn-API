using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class User6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AspNetUsers_ApplicationUserId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_ApplicationUserId",
                table: "Persons");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ApplicationUserId",
                table: "Persons",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AspNetUsers_ApplicationUserId",
                table: "Persons",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AspNetUsers_ApplicationUserId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_ApplicationUserId",
                table: "Persons");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ApplicationUserId",
                table: "Persons",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AspNetUsers_ApplicationUserId",
                table: "Persons",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
