using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate33_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogSource",
                table: "Logs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "agentId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "agentName",
                table: "Logs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "agnetType",
                table: "Logs",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 25, 22, 51, 19, 724, DateTimeKind.Local).AddTicks(7842), new Guid("0b7d36d6-892c-4ff8-8a78-f1a923777501") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogSource",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "agentId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "agentName",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "agnetType",
                table: "Logs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 24, 23, 34, 19, 608, DateTimeKind.Local).AddTicks(8648), new Guid("55d46007-3464-4dcf-9c03-dc31a7ca60df") });
        }
    }
}
