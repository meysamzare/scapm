using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate40_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorizedFullName",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorizedType",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AuthorizedUsername",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorizeState",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMeliCode",
                table: "Attributes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 2, 15, 8, 52, 8, 248, DateTimeKind.Local).AddTicks(6073), new Guid("65ee146c-04cc-4f67-aedf-1f11c836c1db") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizedFullName",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AuthorizedType",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AuthorizedUsername",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AuthorizeState",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsMeliCode",
                table: "Attributes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 12, 28, 21, 42, 4, 322, DateTimeKind.Local).AddTicks(1266), new Guid("772f5575-11c7-414f-90a6-a65c77f576d6") });
        }
    }
}
