using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class Datesfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateRelease",
                table: "Orders",
                newName: "ReleaseDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateToRelease",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateToRelease",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ReleaseDate",
                table: "Orders",
                newName: "DateRelease");
        }
    }
}
