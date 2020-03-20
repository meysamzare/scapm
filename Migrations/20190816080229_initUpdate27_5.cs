using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate27_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Edit_StudentParentPassword",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_StudentPassword",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 16, 12, 32, 28, 554, DateTimeKind.Local).AddTicks(2737), new Guid("ee661ce7-3a53-4eec-b695-d620218cf758") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edit_StudentParentPassword",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_StudentPassword",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 15, 23, 21, 20, 898, DateTimeKind.Local).AddTicks(1395), new Guid("80a4fde3-c7aa-4bd8-8554-29af98a25ff3") });
        }
    }
}
