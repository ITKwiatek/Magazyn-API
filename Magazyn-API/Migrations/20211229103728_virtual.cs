using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazyn_API.Migrations
{
    public partial class @virtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VirtualOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VirtualOrders_Persons_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VirtualItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VirtualOrderId = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    RequiredQuantity = table.Column<int>(type: "int", nullable: false),
                    VirtualOrderModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VirtualItems_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VirtualItems_VirtualOrders_VirtualOrderId",
                        column: x => x.VirtualOrderId,
                        principalTable: "VirtualOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VirtualItems_VirtualOrders_VirtualOrderModelId",
                        column: x => x.VirtualOrderModelId,
                        principalTable: "VirtualOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VirtualManyToMany",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    VirtualOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualManyToMany", x => new { x.OrderId, x.VirtualOrderId });
                    table.ForeignKey(
                        name: "FK_VirtualManyToMany_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VirtualManyToMany_VirtualOrders_VirtualOrderId",
                        column: x => x.VirtualOrderId,
                        principalTable: "VirtualOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VirtualItems_ComponentId",
                table: "VirtualItems",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualItems_VirtualOrderId",
                table: "VirtualItems",
                column: "VirtualOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualItems_VirtualOrderModelId",
                table: "VirtualItems",
                column: "VirtualOrderModelId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualManyToMany_VirtualOrderId",
                table: "VirtualManyToMany",
                column: "VirtualOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualOrders_CreatedById",
                table: "VirtualOrders",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VirtualItems");

            migrationBuilder.DropTable(
                name: "VirtualManyToMany");

            migrationBuilder.DropTable(
                name: "VirtualOrders");
        }
    }
}
