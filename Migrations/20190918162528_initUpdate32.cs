using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tch.Password",
                table: "sm.Teacher",
                nullable: true,
                defaultValue: "1");

            migrationBuilder.AddColumn<bool>(
                name: "Edit_TeacherPassword",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 18, 20, 55, 27, 145, DateTimeKind.Local).AddTicks(560), new Guid("210c99ee-2dbd-46ca-806b-279b933e48d9") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tch.Password",
                table: "sm.Teacher");

            migrationBuilder.DropColumn(
                name: "Edit_TeacherPassword",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 14, 9, 57, 22, 693, DateTimeKind.Local).AddTicks(880), new Guid("498ef5ea-9419-491e-b6ec-8f3aa7935555") });
        }
    }
}
