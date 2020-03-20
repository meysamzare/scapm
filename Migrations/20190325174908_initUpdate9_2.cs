using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate9_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_Class",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Grade",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Class",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Grade",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Class",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Grade",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Class",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Grade",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 25, 22, 19, 7, 829, DateTimeKind.Local).AddTicks(9414), new Guid("25eba012-5340-4d13-8079-8f7366021018") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Add_Class",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Grade",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Class",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Grade",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Class",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Grade",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Class",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Grade",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 25, 21, 48, 8, 830, DateTimeKind.Local).AddTicks(5683), new Guid("8f5170c3-686b-46ec-a6ba-0a144bd9a797") });
        }
    }
}
