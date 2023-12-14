using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace khiemnguyen.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class banuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StatusAccount",
                table: "UserInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "StatusAccount" },
                values: new object[] { new DateTime(2023, 12, 14, 15, 58, 14, 210, DateTimeKind.Local).AddTicks(3276), false });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "StatusAccount" },
                values: new object[] { new DateTime(2023, 12, 14, 15, 58, 14, 210, DateTimeKind.Local).AddTicks(3345), false });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "StatusAccount" },
                values: new object[] { new DateTime(2023, 12, 14, 15, 58, 14, 210, DateTimeKind.Local).AddTicks(3348), false });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 4,
                columns: new[] { "CreatedDate", "StatusAccount" },
                values: new object[] { new DateTime(2023, 12, 14, 15, 58, 14, 210, DateTimeKind.Local).AddTicks(3349), false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusAccount",
                table: "UserInfo");

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 13, 21, 6, 1, 547, DateTimeKind.Local).AddTicks(8287));

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 13, 21, 6, 1, 547, DateTimeKind.Local).AddTicks(8300));

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 13, 21, 6, 1, 547, DateTimeKind.Local).AddTicks(8301));

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 13, 21, 6, 1, 547, DateTimeKind.Local).AddTicks(8302));
        }
    }
}
