using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate47_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlankCount",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FalseCount",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MeliCode",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "Items",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TrueCount",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsStaticDataSaved",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSavedStaticData",
                table: "Categories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "TopScore",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 4, 12, 47, 13, 641, DateTimeKind.Local).AddTicks(8991), new Guid("2bcf7b92-cdc8-44fa-b039-e3f514e907c1") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlankCount",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FalseCount",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MeliCode",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TrueCount",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsStaticDataSaved",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastSavedStaticData",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TopScore",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 3, 11, 35, 25, 916, DateTimeKind.Local).AddTicks(5946), new Guid("72c5827a-67e9-4c66-b699-954f29b14b2e") });
        }
    }
}
