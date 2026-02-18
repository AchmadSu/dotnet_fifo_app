using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FifoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9bd4ff6e-3e26-43fc-a737-6bf299c7f1d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee68bc5f-bb2b-421d-975b-4b358acf3819");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7746f549-b279-459e-a3f3-e36f48b64de7", null, "Admin", "ADMIN" },
                    { "bc1ff846-76ed-41c2-9492-56157f741d8f", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7746f549-b279-459e-a3f3-e36f48b64de7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc1ff846-76ed-41c2-9492-56157f741d8f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9bd4ff6e-3e26-43fc-a737-6bf299c7f1d1", null, "Admin", "ADMIN" },
                    { "ee68bc5f-bb2b-421d-975b-4b358acf3819", null, "User", "USER" }
                });
        }
    }
}
