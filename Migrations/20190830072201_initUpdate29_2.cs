using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate29_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReciverName",
                table: "sm.Ticket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "sm.Ticket",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 30, 11, 52, 0, 631, DateTimeKind.Local).AddTicks(1572), new Guid("bbd25597-4830-430a-94e5-f05fec03d566") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReciverName",
                table: "sm.Ticket");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "sm.Ticket");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 29, 20, 18, 34, 137, DateTimeKind.Local).AddTicks(359), new Guid("93247a59-ab97-4123-9418-1bfbe3784de0") });
        }
    }
}
