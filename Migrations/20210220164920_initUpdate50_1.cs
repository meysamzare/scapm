using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate50_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterItemStepType",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 20, 20, 19, 19, 890, DateTimeKind.Local).AddTicks(1122), new Guid("73f39ff5-a3b2-4e1b-8d47-82cefc784fe0") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterItemStepType",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 11, 15, 56, 16, 133, DateTimeKind.Local).AddTicks(4250), new Guid("72b76f38-813f-46a9-95f1-af72b054b20e") });
        }
    }
}
