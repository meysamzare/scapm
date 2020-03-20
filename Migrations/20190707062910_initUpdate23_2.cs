using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate23_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_Advertising",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Advertising",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Advertising",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Advertising",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 7, 10, 59, 9, 221, DateTimeKind.Local).AddTicks(4081), new Guid("12bd480c-934a-4947-931d-caab763acc42") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Add_Advertising",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Advertising",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Advertising",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Advertising",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 7, 10, 43, 4, 116, DateTimeKind.Local).AddTicks(1497), new Guid("d7a08b9e-6b63-4a08-9b94-e4b4753df5f7") });
        }
    }
}
