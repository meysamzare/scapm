using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate49_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_Courses_CourseId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_Grades_GradeId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_OnlineClassServers_OnlineClassServerId",
                table: "OnlineClasses");

            migrationBuilder.AlterColumn<int>(
                name: "OnlineClassServerId",
                table: "OnlineClasses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "OnlineClasses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "OnlineClasses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AllowedAdminIds",
                table: "OnlineClasses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllowedStudentIds",
                table: "OnlineClasses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorizeType",
                table: "OnlineClasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 6, 22, 7, 34, 304, DateTimeKind.Local).AddTicks(9679), new Guid("15d1812e-5c75-4b16-b4f7-7aab8f58696c") });

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_Courses_CourseId",
                table: "OnlineClasses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_Grades_GradeId",
                table: "OnlineClasses",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_OnlineClassServers_OnlineClassServerId",
                table: "OnlineClasses",
                column: "OnlineClassServerId",
                principalTable: "OnlineClassServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_Courses_CourseId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_Grades_GradeId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_OnlineClassServers_OnlineClassServerId",
                table: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "AllowedAdminIds",
                table: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "AllowedStudentIds",
                table: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "AuthorizeType",
                table: "OnlineClasses");

            migrationBuilder.AlterColumn<int>(
                name: "OnlineClassServerId",
                table: "OnlineClasses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "OnlineClasses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "OnlineClasses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 5, 21, 59, 1, 410, DateTimeKind.Local).AddTicks(6401), new Guid("e080c600-d9d0-419b-a786-0cd79cca86e0") });

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_Courses_CourseId",
                table: "OnlineClasses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_Grades_GradeId",
                table: "OnlineClasses",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_OnlineClassServers_OnlineClassServerId",
                table: "OnlineClasses",
                column: "OnlineClassServerId",
                principalTable: "OnlineClassServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
