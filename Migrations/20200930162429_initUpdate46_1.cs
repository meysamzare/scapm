using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate46_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EasyQuestionNumber",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HardQuestionNumber",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerateQuestionNumber",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseLimitedRandomQuestionNumber",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VeryHardQuestionNumber",
                table: "Categories",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 9, 30, 19, 54, 26, 687, DateTimeKind.Local).AddTicks(9529), new Guid("9d9e7b75-eca0-4133-8880-b7eeb7ac7ed0") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EasyQuestionNumber",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HardQuestionNumber",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModerateQuestionNumber",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UseLimitedRandomQuestionNumber",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "VeryHardQuestionNumber",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 9, 25, 15, 45, 16, 35, DateTimeKind.Local).AddTicks(9098), new Guid("975c5178-27f5-4684-8f27-0d94c1a8f8b2") });
        }
    }
}
