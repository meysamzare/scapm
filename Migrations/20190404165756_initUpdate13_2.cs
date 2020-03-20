using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate13_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ex.BlankAnswer",
                table: "sm.ExamScore",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 4, 21, 27, 55, 696, DateTimeKind.Local).AddTicks(6423), new Guid("166eb1ce-b3ac-453c-96f7-229a25e272a6") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ex.BlankAnswer",
                table: "sm.ExamScore");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 3, 1, 12, 53, 156, DateTimeKind.Local).AddTicks(6017), new Guid("dedd0c20-1cfc-4a13-9601-10c20cd4c071") });
        }
    }
}
