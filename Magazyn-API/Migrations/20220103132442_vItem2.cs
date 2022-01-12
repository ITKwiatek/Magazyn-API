using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class vItem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VirtualItems_VirtualOrders_VirtualOrderId1",
                table: "VirtualItems");

            migrationBuilder.DropIndex(
                name: "IX_VirtualItems_VirtualOrderId1",
                table: "VirtualItems");

            migrationBuilder.DropColumn(
                name: "VirtualOrderId1",
                table: "VirtualItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VirtualOrderId1",
                table: "VirtualItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VirtualItems_VirtualOrderId1",
                table: "VirtualItems",
                column: "VirtualOrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualItems_VirtualOrders_VirtualOrderId1",
                table: "VirtualItems",
                column: "VirtualOrderId1",
                principalTable: "VirtualOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
