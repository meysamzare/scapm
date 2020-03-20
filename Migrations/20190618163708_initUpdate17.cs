using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BtnTitle",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HaveEntringCard",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 18, 21, 7, 7, 716, DateTimeKind.Local).AddTicks(2831), new Guid("ef2c0bf1-611b-428a-bb46-f44ae97c2270") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BtnTitle",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HaveEntringCard",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 1, 11, 53, 27, 513, DateTimeKind.Local).AddTicks(3270), new Guid("9133f577-386f-47c7-9fea-9fcca469f2d4") });
        }
    }
}
