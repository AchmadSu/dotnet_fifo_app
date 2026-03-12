using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FifoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTempIdToSaleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d969828d-e450-4e27-885e-9315ac666a1c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb3de6fd-1022-46ba-a9a2-86efcd58a8df");

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "SaleItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5a958f21-39b2-4082-a8d6-5d07ad75537a", null, "Admin", "ADMIN" },
                    { "f0a3cc3c-63da-424e-89c6-4fbb61a9995d", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a958f21-39b2-4082-a8d6-5d07ad75537a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0a3cc3c-63da-424e-89c6-4fbb61a9995d");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "SaleItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d969828d-e450-4e27-885e-9315ac666a1c", null, "Admin", "ADMIN" },
                    { "fb3de6fd-1022-46ba-a9a2-86efcd58a8df", null, "User", "USER" }
                });
        }
    }
}
