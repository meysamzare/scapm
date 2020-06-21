using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_OnlineExam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_OnlineExamOption",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_OnlineExamResult",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_OnlineExam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_OnlineExamOption",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_OnlineExamResult",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_OnlineExam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_OnlineExamOption",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_OnlineExamResult",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_OnlineExam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_OnlineExamOption",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_OnlineExamResult",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RandomAttribute",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RandomAttributeOption",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Attributes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 5, 17, 4, 25, 29, 316, DateTimeKind.Local).AddTicks(7237), new Guid("825a7af2-cb8f-4014-9580-c6c360534c0c") });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ClassId",
                table: "Categories",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_GradeId",
                table: "Categories",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_QuestionId",
                table: "Attributes",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_sm.Questions_QuestionId",
                table: "Attributes",
                column: "QuestionId",
                principalTable: "sm.Questions",
                principalColumn: "Que.id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_sm.Class_ClassId",
                table: "Categories",
                column: "ClassId",
                principalTable: "sm.Class",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_sm.Grade_GradeId",
                table: "Categories",
                column: "GradeId",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_sm.Questions_QuestionId",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_sm.Class_ClassId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_sm.Grade_GradeId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ClassId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_GradeId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_QuestionId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "Add_OnlineExam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_OnlineExamOption",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_OnlineExamResult",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_OnlineExam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_OnlineExamOption",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_OnlineExamResult",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_OnlineExam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_OnlineExamOption",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_OnlineExamResult",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_OnlineExam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_OnlineExamOption",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_OnlineExamResult",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RandomAttribute",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RandomAttributeOption",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Attributes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 5, 9, 14, 10, 9, 418, DateTimeKind.Local).AddTicks(4937), new Guid("0030f7a0-c228-4dc8-b91a-8b0d7d0c744e") });
        }
    }
}
