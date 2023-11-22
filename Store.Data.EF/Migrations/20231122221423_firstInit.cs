using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class firstInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isbn = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryServiceName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DeliveryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryPrice = table.Column<decimal>(type: "money", nullable: false),
                    DeliveryParameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentServiceName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PaymentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentParameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    OrderDtoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemDto_OrderDto_OrderDtoId",
                        column: x => x.OrderDtoId,
                        principalTable: "OrderDto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BookDto",
                columns: new[] { "Id", "Author", "Description", "Isbn", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Kevin", "some book", "ISBN 12345-12345", 20m, "True" },
                    { 2, "Justin", "anouther book", "ISBN 12345-12346", 30m, "Forewer" },
                    { 3, "Jack", "third book", "ISBN 12345-67891", 10m, "False" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemDto_OrderDtoId",
                table: "OrderItemDto",
                column: "OrderDtoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookDto");

            migrationBuilder.DropTable(
                name: "OrderItemDto");

            migrationBuilder.DropTable(
                name: "OrderDto");
        }
    }
}
