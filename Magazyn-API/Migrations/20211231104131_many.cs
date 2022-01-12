using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VirtualManyToMany_Orders_OrderId",
                table: "VirtualManyToMany");

            migrationBuilder.DropForeignKey(
                name: "FK_VirtualManyToMany_VirtualOrders_VirtualOrderId",
                table: "VirtualManyToMany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VirtualManyToMany",
                table: "VirtualManyToMany");

            migrationBuilder.RenameTable(
                name: "VirtualManyToMany",
                newName: "ManyToMany");

            migrationBuilder.RenameIndex(
                name: "IX_VirtualManyToMany_VirtualOrderId",
                table: "ManyToMany",
                newName: "IX_ManyToMany_VirtualOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManyToMany",
                table: "ManyToMany",
                columns: new[] { "OrderId", "VirtualOrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ManyToMany_Orders_OrderId",
                table: "ManyToMany",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManyToMany_VirtualOrders_VirtualOrderId",
                table: "ManyToMany",
                column: "VirtualOrderId",
                principalTable: "VirtualOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManyToMany_Orders_OrderId",
                table: "ManyToMany");

            migrationBuilder.DropForeignKey(
                name: "FK_ManyToMany_VirtualOrders_VirtualOrderId",
                table: "ManyToMany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ManyToMany",
                table: "ManyToMany");

            migrationBuilder.RenameTable(
                name: "ManyToMany",
                newName: "VirtualManyToMany");

            migrationBuilder.RenameIndex(
                name: "IX_ManyToMany_VirtualOrderId",
                table: "VirtualManyToMany",
                newName: "IX_VirtualManyToMany_VirtualOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VirtualManyToMany",
                table: "VirtualManyToMany",
                columns: new[] { "OrderId", "VirtualOrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualManyToMany_Orders_OrderId",
                table: "VirtualManyToMany",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualManyToMany_VirtualOrders_VirtualOrderId",
                table: "VirtualManyToMany",
                column: "VirtualOrderId",
                principalTable: "VirtualOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
