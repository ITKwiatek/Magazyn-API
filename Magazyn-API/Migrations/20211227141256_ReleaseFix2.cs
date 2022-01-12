using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class ReleaseFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReleaseItems_OrderItemId",
                table: "ReleaseItems");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId1",
                table: "ReleaseItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseItems_OrderItemId",
                table: "ReleaseItems",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseItems_OrderItemId1",
                table: "ReleaseItems",
                column: "OrderItemId1",
                unique: true,
                filter: "[OrderItemId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseItems_OrderItems_OrderItemId1",
                table: "ReleaseItems",
                column: "OrderItemId1",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseItems_OrderItems_OrderItemId1",
                table: "ReleaseItems");

            migrationBuilder.DropIndex(
                name: "IX_ReleaseItems_OrderItemId",
                table: "ReleaseItems");

            migrationBuilder.DropIndex(
                name: "IX_ReleaseItems_OrderItemId1",
                table: "ReleaseItems");

            migrationBuilder.DropColumn(
                name: "OrderItemId1",
                table: "ReleaseItems");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseItems_OrderItemId",
                table: "ReleaseItems",
                column: "OrderItemId",
                unique: true);
        }
    }
}
