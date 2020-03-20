using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate13_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.Class_sm.Grade_Grd.Id",
                table: "sm.Class");

            migrationBuilder.AddColumn<bool>(
                name: "Add_Exam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_ExamScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_ExamType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Question",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_QuestionOption",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Exam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_ExamScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_ExamType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Question",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_QuestionOption",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Exam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_ExamScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_ExamType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Question",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_QuestionOption",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Exam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_ExamScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_ExamType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Question",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_QuestionOption",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 3, 1, 12, 53, 156, DateTimeKind.Local).AddTicks(6017), new Guid("dedd0c20-1cfc-4a13-9601-10c20cd4c071") });

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Class_sm.Grade_Grd.Id",
                table: "sm.Class",
                column: "Grd.Id",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.Class_sm.Grade_Grd.Id",
                table: "sm.Class");

            migrationBuilder.DropColumn(
                name: "Add_Exam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_ExamScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_ExamType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Question",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_QuestionOption",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Exam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_ExamScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_ExamType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Question",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_QuestionOption",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Exam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_ExamScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_ExamType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Question",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_QuestionOption",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Exam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_ExamScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_ExamType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Question",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_QuestionOption",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 2, 13, 41, 54, 200, DateTimeKind.Local).AddTicks(1480), new Guid("f5fabf16-89fa-44af-99a2-ef5b0486abc6") });

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Class_sm.Grade_Grd.Id",
                table: "sm.Class",
                column: "Grd.Id",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
