using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BookRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRatings_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 13,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 14,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 15,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 16,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 17,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 18,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 19,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 20,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 21,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 22,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 23,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 24,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 25,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 26,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 27,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 28,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 29,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 30,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 31,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 32,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 33,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 34,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 35,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 36,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 37,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 38,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 39,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 40,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 41,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 42,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 43,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 44,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 45,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 46,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 47,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 48,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 49,
                column: "Summary",
                value: "");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 50,
                column: "Summary",
                value: "");

            migrationBuilder.CreateIndex(
                name: "IX_BookRatings_BookId_UserId",
                table: "BookRatings",
                columns: new[] { "BookId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRatings_UserId",
                table: "BookRatings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookRatings");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Books");
        }
    }
}
