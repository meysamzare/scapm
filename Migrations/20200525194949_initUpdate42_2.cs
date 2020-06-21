using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate42_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComplatabelContent",
                table: "sm.Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "sm.QuestionOptions",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 5, 26, 0, 19, 48, 401, DateTimeKind.Local).AddTicks(2332), new Guid("4a4e56ec-22c3-4de6-8ae0-ce477426cc42") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplatabelContent",
                table: "sm.Questions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "sm.QuestionOptions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 5, 17, 11, 21, 38, 426, DateTimeKind.Local).AddTicks(8941), new Guid("426afadc-085a-41b3-93d9-eb4f3806afb4") });
        }
    }
}
