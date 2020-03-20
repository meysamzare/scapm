using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveMessage",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeActiveMessage",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HaveInfo",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInfoShow",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInClient",
                table: "Attributes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInShowInfo",
                table: "Attributes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 27, 16, 8, 32, 197, DateTimeKind.Local).AddTicks(7007), new Guid("d6530a1d-347c-42b7-a76f-056aa801217e") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveMessage",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeActiveMessage",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HaveInfo",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsInfoShow",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsInClient",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "IsInShowInfo",
                table: "Attributes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 13, 21, 6, 38, 996, DateTimeKind.Local).AddTicks(3356), new Guid("030e09bf-d55f-413e-bebf-382e0a8462bf") });
        }
    }
}
