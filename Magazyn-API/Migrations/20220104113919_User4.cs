using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class User4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AspNetUsers_ApplicationUserId1",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_ApplicationUserId1",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Persons");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ApplicationUserId",
                table: "Persons",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AspNetUsers_ApplicationUserId",
                table: "Persons",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AspNetUsers_ApplicationUserId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_ApplicationUserId",
                table: "Persons");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Persons",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ApplicationUserId1",
                table: "Persons",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AspNetUsers_ApplicationUserId1",
                table: "Persons",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
