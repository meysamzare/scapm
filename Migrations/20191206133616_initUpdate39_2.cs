using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate39_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancellReason",
                table: "sm.Exams",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "sm.Exams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "NotificationSeens",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "NotificationSeens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "NotificationSeens",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 12, 6, 17, 6, 15, 106, DateTimeKind.Local).AddTicks(4863), new Guid("e836d5d9-2eae-482e-9dc6-4db7788919c8") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellReason",
                table: "sm.Exams");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "sm.Exams");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "NotificationSeens");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "NotificationSeens");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "NotificationSeens");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 27, 16, 33, 31, 5, DateTimeKind.Local).AddTicks(8594), new Guid("63677421-0e35-4caa-906c-ab5e7b3a1c04") });
        }
    }
}
