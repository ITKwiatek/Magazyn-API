using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class virtual4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VirtualItems_VirtualOrders_VirtualOrderId1",
                table: "VirtualItems");

            migrationBuilder.RenameColumn(
                name: "VirtualOrderId1",
                table: "VirtualItems",
                newName: "VirtualOrderModelId");

            migrationBuilder.RenameIndex(
                name: "IX_VirtualItems_VirtualOrderId1",
                table: "VirtualItems",
                newName: "IX_VirtualItems_VirtualOrderModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualItems_VirtualOrders_VirtualOrderModelId",
                table: "VirtualItems",
                column: "VirtualOrderModelId",
                principalTable: "VirtualOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VirtualItems_VirtualOrders_VirtualOrderModelId",
                table: "VirtualItems");

            migrationBuilder.RenameColumn(
                name: "VirtualOrderModelId",
                table: "VirtualItems",
                newName: "VirtualOrderId1");

            migrationBuilder.RenameIndex(
                name: "IX_VirtualItems_VirtualOrderModelId",
                table: "VirtualItems",
                newName: "IX_VirtualItems_VirtualOrderId1");

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
