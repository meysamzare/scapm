using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate39_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllCourseAccess",
                table: "sm.Teacher",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_TeacherAllAccess",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 27, 16, 33, 31, 5, DateTimeKind.Local).AddTicks(8594), new Guid("63677421-0e35-4caa-906c-ab5e7b3a1c04") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllCourseAccess",
                table: "sm.Teacher");

            migrationBuilder.DropColumn(
                name: "Edit_TeacherAllAccess",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 21, 18, 17, 29, 404, DateTimeKind.Local).AddTicks(1808), new Guid("a60f0902-1f62-46e3-b706-01a8d2c1635a") });
        }
    }
}
