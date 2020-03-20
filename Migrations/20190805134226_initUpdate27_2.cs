using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate27_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_StudentType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_StudentType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_StudentType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_StudentType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 5, 18, 12, 25, 223, DateTimeKind.Local).AddTicks(8025), new Guid("0ef6a271-b9af-46c1-9d7d-d5e796671f4f") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Add_StudentType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_StudentType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_StudentType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_StudentType",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 5, 18, 4, 9, 335, DateTimeKind.Local).AddTicks(1150), new Guid("794b2a3e-8c81-4f58-924b-c9cec5c0d101") });
        }
    }
}
