using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate27_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentsPassword",
                table: "sm.Student",
                nullable: true,
                defaultValue: "1");

            migrationBuilder.AddColumn<string>(
                name: "StudentPassword",
                table: "sm.Student",
                nullable: true,
                defaultValue: "1");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 15, 23, 21, 20, 898, DateTimeKind.Local).AddTicks(1395), new Guid("80a4fde3-c7aa-4bd8-8554-29af98a25ff3") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentsPassword",
                table: "sm.Student");

            migrationBuilder.DropColumn(
                name: "StudentPassword",
                table: "sm.Student");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 14, 21, 17, 42, 187, DateTimeKind.Local).AddTicks(3367), new Guid("f146ccf9-cf0a-4a6e-9dde-25fcac232de7") });
        }
    }
}
