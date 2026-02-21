using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FifoApi.Migrations
{
    /// <inheritdoc />
    public partial class ReconfigureModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ca0d8f8-ad59-4081-a8e7-f90b4f97cdae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27ae855d-534f-44fb-89fe-4f69a0d5c93f");

            migrationBuilder.AddColumn<string>(
                name: "CreatorAppUserId",
                table: "StockMovements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "StockMovements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "StockMovements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierAppUserId",
                table: "StockMovements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorAppUserId",
                table: "StockBatches",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "StockBatches",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "StockBatches",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierAppUserId",
                table: "StockBatches",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorAppUserId",
                table: "Sales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Sales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "Sales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierAppUserId",
                table: "Sales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorAppUserId",
                table: "SaleItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "SaleItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "SaleItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierAppUserId",
                table: "SaleItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorAppUserId",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierAppUserId",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "99244831-fc1e-4af7-bc13-aadca59e7710", null, "User", "USER" },
                    { "d65b6272-7762-43e8-b4bb-92c6783c831a", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_CreatorAppUserId",
                table: "StockMovements",
                column: "CreatorAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ModifierAppUserId",
                table: "StockMovements",
                column: "ModifierAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockBatches_CreatorAppUserId",
                table: "StockBatches",
                column: "CreatorAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockBatches_ModifierAppUserId",
                table: "StockBatches",
                column: "ModifierAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CreatorAppUserId",
                table: "Sales",
                column: "CreatorAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ModifierAppUserId",
                table: "Sales",
                column: "ModifierAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_CreatorAppUserId",
                table: "SaleItems",
                column: "CreatorAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ModifierAppUserId",
                table: "SaleItems",
                column: "ModifierAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatorAppUserId",
                table: "Products",
                column: "CreatorAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ModifierAppUserId",
                table: "Products",
                column: "ModifierAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_CreatorAppUserId",
                table: "Products",
                column: "CreatorAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_ModifierAppUserId",
                table: "Products",
                column: "ModifierAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_AspNetUsers_CreatorAppUserId",
                table: "SaleItems",
                column: "CreatorAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_AspNetUsers_ModifierAppUserId",
                table: "SaleItems",
                column: "ModifierAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_AspNetUsers_CreatorAppUserId",
                table: "Sales",
                column: "CreatorAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_AspNetUsers_ModifierAppUserId",
                table: "Sales",
                column: "ModifierAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatches_AspNetUsers_CreatorAppUserId",
                table: "StockBatches",
                column: "CreatorAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatches_AspNetUsers_ModifierAppUserId",
                table: "StockBatches",
                column: "ModifierAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_AspNetUsers_CreatorAppUserId",
                table: "StockMovements",
                column: "CreatorAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_AspNetUsers_ModifierAppUserId",
                table: "StockMovements",
                column: "ModifierAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_CreatorAppUserId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_ModifierAppUserId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_AspNetUsers_CreatorAppUserId",
                table: "SaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_AspNetUsers_ModifierAppUserId",
                table: "SaleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_AspNetUsers_CreatorAppUserId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_AspNetUsers_ModifierAppUserId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBatches_AspNetUsers_CreatorAppUserId",
                table: "StockBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBatches_AspNetUsers_ModifierAppUserId",
                table: "StockBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_AspNetUsers_CreatorAppUserId",
                table: "StockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_AspNetUsers_ModifierAppUserId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_CreatorAppUserId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_ModifierAppUserId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockBatches_CreatorAppUserId",
                table: "StockBatches");

            migrationBuilder.DropIndex(
                name: "IX_StockBatches_ModifierAppUserId",
                table: "StockBatches");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CreatorAppUserId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ModifierAppUserId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_SaleItems_CreatorAppUserId",
                table: "SaleItems");

            migrationBuilder.DropIndex(
                name: "IX_SaleItems_ModifierAppUserId",
                table: "SaleItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreatorAppUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ModifierAppUserId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99244831-fc1e-4af7-bc13-aadca59e7710");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d65b6272-7762-43e8-b4bb-92c6783c831a");

            migrationBuilder.DropColumn(
                name: "CreatorAppUserId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "ModifierAppUserId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "CreatorAppUserId",
                table: "StockBatches");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "StockBatches");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "StockBatches");

            migrationBuilder.DropColumn(
                name: "ModifierAppUserId",
                table: "StockBatches");

            migrationBuilder.DropColumn(
                name: "CreatorAppUserId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ModifierAppUserId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CreatorAppUserId",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "ModifierAppUserId",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "CreatorAppUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModifierAppUserId",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ca0d8f8-ad59-4081-a8e7-f90b4f97cdae", null, "User", "USER" },
                    { "27ae855d-534f-44fb-89fe-4f69a0d5c93f", null, "Admin", "ADMIN" }
                });
        }
    }
}
