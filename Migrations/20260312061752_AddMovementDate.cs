using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FifoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMovementDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a958f21-39b2-4082-a8d6-5d07ad75537a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0a3cc3c-63da-424e-89c6-4fbb61a9995d");

            migrationBuilder.AddColumn<DateTime>(
                name: "MovementDate",
                table: "StockMovements",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24fa05b5-ed7e-4a0c-9e31-644636439eec", null, "Admin", "ADMIN" },
                    { "feb4ee28-db5f-4986-a564-b48c2680ed44", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24fa05b5-ed7e-4a0c-9e31-644636439eec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "feb4ee28-db5f-4986-a564-b48c2680ed44");

            migrationBuilder.DropColumn(
                name: "MovementDate",
                table: "StockMovements");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5a958f21-39b2-4082-a8d6-5d07ad75537a", null, "Admin", "ADMIN" },
                    { "f0a3cc3c-63da-424e-89c6-4fbb61a9995d", null, "User", "USER" }
                });
        }
    }
}
