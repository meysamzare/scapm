using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate50_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPhoneNumber",
                table: "Attributes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequiredErrorMessage",
                table: "Attributes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqErrorMessage",
                table: "Attributes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 21, 20, 19, 23, 146, DateTimeKind.Local).AddTicks(3591), new Guid("40949c3b-09a4-43b1-b6bf-94cedc04d224") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPhoneNumber",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "RequiredErrorMessage",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UniqErrorMessage",
                table: "Attributes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 20, 20, 19, 19, 890, DateTimeKind.Local).AddTicks(1122), new Guid("73f39ff5-a3b2-4e1b-8d47-82cefc784fe0") });
        }
    }
}
