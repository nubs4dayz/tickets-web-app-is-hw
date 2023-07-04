using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Repository.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketInOrder_Order_TicketId",
                table: "TicketInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInOrder_Tickets_OrderId",
                table: "TicketInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInShoppingCarts_ShoppingCarts_TicketId",
                table: "TicketInShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInShoppingCarts_Tickets_ShoppingCartId",
                table: "TicketInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInShoppingCarts",
                table: "TicketInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInOrder",
                table: "TicketInOrder");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TicketInOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInShoppingCarts",
                table: "TicketInShoppingCarts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInOrder",
                table: "TicketInOrder",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInShoppingCarts_TicketId",
                table: "TicketInShoppingCarts",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInOrder_TicketId",
                table: "TicketInOrder",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInOrder_Order_OrderId",
                table: "TicketInOrder",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInOrder_Tickets_TicketId",
                table: "TicketInOrder",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInShoppingCarts_ShoppingCarts_ShoppingCartId",
                table: "TicketInShoppingCarts",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInShoppingCarts_Tickets_TicketId",
                table: "TicketInShoppingCarts",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketInOrder_Order_OrderId",
                table: "TicketInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInOrder_Tickets_TicketId",
                table: "TicketInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInShoppingCarts_ShoppingCarts_ShoppingCartId",
                table: "TicketInShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInShoppingCarts_Tickets_TicketId",
                table: "TicketInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInShoppingCarts",
                table: "TicketInShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_TicketInShoppingCarts_TicketId",
                table: "TicketInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInOrder",
                table: "TicketInOrder");

            migrationBuilder.DropIndex(
                name: "IX_TicketInOrder_TicketId",
                table: "TicketInOrder");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TicketInOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInShoppingCarts",
                table: "TicketInShoppingCarts",
                columns: new[] { "TicketId", "ShoppingCartId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInOrder",
                table: "TicketInOrder",
                columns: new[] { "TicketId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInOrder_Order_TicketId",
                table: "TicketInOrder",
                column: "TicketId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInOrder_Tickets_OrderId",
                table: "TicketInOrder",
                column: "OrderId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInShoppingCarts_ShoppingCarts_TicketId",
                table: "TicketInShoppingCarts",
                column: "TicketId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInShoppingCarts_Tickets_ShoppingCartId",
                table: "TicketInShoppingCarts",
                column: "ShoppingCartId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
