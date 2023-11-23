using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemDto_OrderDto_OrderDtoId",
                table: "OrderItemDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItemDto",
                table: "OrderItemDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDto",
                table: "OrderDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookDto",
                table: "BookDto");

            migrationBuilder.RenameTable(
                name: "OrderItemDto",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "OrderDto",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "BookDto",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemDto_OrderDtoId",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderDtoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderDtoId",
                table: "OrderItems",
                column: "OrderDtoId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderDtoId",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "OrderDto");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItemDto");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "BookDto");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderDtoId",
                table: "OrderItemDto",
                newName: "IX_OrderItemDto_OrderDtoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDto",
                table: "OrderDto",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItemDto",
                table: "OrderItemDto",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookDto",
                table: "BookDto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemDto_OrderDto_OrderDtoId",
                table: "OrderItemDto",
                column: "OrderDtoId",
                principalTable: "OrderDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
