using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FifoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeIndexStockMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e8180d0-898c-44f7-848f-c50d9d54669d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2fd6b16d-7713-4df2-9137-c614791570c6");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "StockMovements",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "StockBatches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "SaleItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "949b0504-5484-4cf0-97c7-688c3b753b31", null, "Admin", "ADMIN" },
                    { "ebf0d42b-1d8b-46a2-8b20-ddc7c18b502d", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_SaleItemId_StockBatchId",
                table: "StockMovements",
                columns: new[] { "SaleItemId", "StockBatchId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockMovements_SaleItemId_StockBatchId",
                table: "StockMovements");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "949b0504-5484-4cf0-97c7-688c3b753b31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ebf0d42b-1d8b-46a2-8b20-ddc7c18b502d");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "StockBatches");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e8180d0-898c-44f7-848f-c50d9d54669d", null, "User", "USER" },
                    { "2fd6b16d-7713-4df2-9137-c614791570c6", null, "Admin", "ADMIN" }
                });
        }
    }
}
