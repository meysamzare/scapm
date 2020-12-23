using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate47_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OnlineExamItemId",
                table: "ExamScores",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 15, 8, 20, 53, 241, DateTimeKind.Local).AddTicks(7904), new Guid("026af37b-c02c-431a-b4aa-19a98788e77f") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnlineExamItemId",
                table: "ExamScores");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 13, 22, 1, 3, 781, DateTimeKind.Local).AddTicks(3206), new Guid("2de07199-9680-4b83-91b4-981066f40d08") });
        }
    }
}
