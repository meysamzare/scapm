using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate47_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_DescriptiveScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_DescriptiveScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_DescriptiveScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_DescriptiveScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 3, 11, 35, 25, 916, DateTimeKind.Local).AddTicks(5946), new Guid("72c5827a-67e9-4c66-b699-954f29b14b2e") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Add_DescriptiveScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_DescriptiveScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_DescriptiveScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_DescriptiveScore",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 10, 27, 22, 41, 58, 212, DateTimeKind.Local).AddTicks(7164), new Guid("5ad5bf14-28a3-4d4d-b0b1-3e7cc5c49363") });
        }
    }
}
