using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FifoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexInvoiceNoToSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f642669-5693-415c-ad77-174daccc78d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c39f11c6-97b1-4494-acf5-265c9879501d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d969828d-e450-4e27-885e-9315ac666a1c", null, "Admin", "ADMIN" },
                    { "fb3de6fd-1022-46ba-a9a2-86efcd58a8df", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d969828d-e450-4e27-885e-9315ac666a1c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb3de6fd-1022-46ba-a9a2-86efcd58a8df");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f642669-5693-415c-ad77-174daccc78d3", null, "User", "USER" },
                    { "c39f11c6-97b1-4494-acf5-265c9879501d", null, "Admin", "ADMIN" }
                });
        }
    }
}
