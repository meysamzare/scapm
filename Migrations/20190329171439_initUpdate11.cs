using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_Tsch.TeacherCode",
                table: "sm.TimeSchedule");

            migrationBuilder.AddColumn<int>(
                name: "TeacherId1",
                table: "sm.TimeSchedule",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Course",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Education",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Insurance",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_OrgChart",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_OrgPerson",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Salary",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Student",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Teacher",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_TimeSchedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_TimeandDays",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Course",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Education",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Insurance",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_OrgChart",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_OrgPerson",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Salary",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Student",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Teacher",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_TimeSchedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_TimeandDays",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Course",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Education",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Insurance",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_OrgChart",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_OrgPerson",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Salary",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Student",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Teacher",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_TimeSchedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_TimeandDays",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Course",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Education",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Insurance",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_OrgChart",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_OrgPerson",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Salary",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Student",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Teacher",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_TimeSchedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_TimeandDays",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 29, 21, 44, 39, 231, DateTimeKind.Local).AddTicks(3685), new Guid("6606ea60-b13e-481f-8841-7e174f1db8f0") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.TimeSchedule_TeacherId1",
                table: "sm.TimeSchedule",
                column: "TeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_Tsch.TeacherCode",
                table: "sm.TimeSchedule",
                column: "Tsch.TeacherCode",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_TeacherId1",
                table: "sm.TimeSchedule",
                column: "TeacherId1",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_Tsch.TeacherCode",
                table: "sm.TimeSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_TeacherId1",
                table: "sm.TimeSchedule");

            migrationBuilder.DropIndex(
                name: "IX_sm.TimeSchedule_TeacherId1",
                table: "sm.TimeSchedule");

            migrationBuilder.DropColumn(
                name: "TeacherId1",
                table: "sm.TimeSchedule");

            migrationBuilder.DropColumn(
                name: "Add_Course",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Education",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Insurance",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_OrgChart",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_OrgPerson",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Salary",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Student",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Teacher",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_TimeSchedule",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_TimeandDays",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Course",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Education",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Insurance",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_OrgChart",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_OrgPerson",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Salary",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Student",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Teacher",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_TimeSchedule",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_TimeandDays",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Course",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Education",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Insurance",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_OrgChart",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_OrgPerson",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Salary",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Student",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Teacher",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_TimeSchedule",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_TimeandDays",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Course",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Education",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Insurance",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_OrgChart",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_OrgPerson",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Salary",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Student",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Teacher",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_TimeSchedule",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_TimeandDays",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 28, 12, 28, 55, 946, DateTimeKind.Local).AddTicks(7169), new Guid("bbdc666a-dc18-4307-8a8d-bd596cd23fe7") });

            migrationBuilder.AddForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_Tsch.TeacherCode",
                table: "sm.TimeSchedule",
                column: "Tsch.TeacherCode",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
