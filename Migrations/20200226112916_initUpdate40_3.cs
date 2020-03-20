using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate40_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_VirtualTeaching",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_VirtualTeaching",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_VirtualTeaching",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_VirtualTeaching",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalType",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 2, 26, 14, 59, 14, 866, DateTimeKind.Local).AddTicks(5815), new Guid("b0685dfb-088a-49ea-b4cc-c64ae9d06ae6") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Add_VirtualTeaching",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_VirtualTeaching",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_VirtualTeaching",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_VirtualTeaching",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "TotalType",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 2, 17, 3, 11, 17, 658, DateTimeKind.Local).AddTicks(6679), new Guid("2669cb0f-b1f3-422c-8740-e81a255bb209") });
        }
    }
}
