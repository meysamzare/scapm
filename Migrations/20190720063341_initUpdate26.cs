using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Std.PictureUrl",
                table: "sm.Student",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "View_StudentStudyRecord",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[] { 1, "NowYeareducationId", "1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 20, 11, 3, 40, 407, DateTimeKind.Local).AddTicks(7907), new Guid("b604c953-8312-4da7-adb7-c4837e98ffe6") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Std.PictureUrl",
                table: "sm.Student");

            migrationBuilder.DropColumn(
                name: "View_StudentStudyRecord",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 17, 1, 0, 18, 262, DateTimeKind.Local).AddTicks(9025), new Guid("5e744403-ec66-4fbe-81e4-256c17477c92") });
        }
    }
}
