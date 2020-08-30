using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate44_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CalculateNegativeScore",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 8, 26, 7, 29, 29, 68, DateTimeKind.Local).AddTicks(8953), new Guid("a25f8872-b597-49a0-9485-2c6585ca84f1") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculateNegativeScore",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 8, 12, 12, 27, 21, 916, DateTimeKind.Local).AddTicks(8012), new Guid("a69a5d8e-197f-4a21-a756-5fd73c2f7bb8") });
        }
    }
}
