using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate44_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Categories_CategoryId",
                table: "Attributes");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamTypeId",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkbookId",
                table: "Categories",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Attributes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsTemplate",
                table: "Attributes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 7, 9, 14, 44, 12, 879, DateTimeKind.Local).AddTicks(6900), new Guid("589907fa-017c-4bf8-9ee8-824f0c5b007d") });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CourseId",
                table: "Categories",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ExamTypeId",
                table: "Categories",
                column: "ExamTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_WorkbookId",
                table: "Categories",
                column: "WorkbookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Categories_CategoryId",
                table: "Attributes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_sm.Course_CourseId",
                table: "Categories",
                column: "CourseId",
                principalTable: "sm.Course",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_sm.ExamTyp_ExamTypeId",
                table: "Categories",
                column: "ExamTypeId",
                principalTable: "sm.ExamTyp",
                principalColumn: "Extyp.id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Workbooks_WorkbookId",
                table: "Categories",
                column: "WorkbookId",
                principalTable: "Workbooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Categories_CategoryId",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_sm.Course_CourseId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_sm.ExamTyp_ExamTypeId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Workbooks_WorkbookId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CourseId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ExamTypeId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_WorkbookId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ExamTypeId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "WorkbookId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsTemplate",
                table: "Attributes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Attributes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 6, 19, 11, 52, 34, 137, DateTimeKind.Local).AddTicks(8558), new Guid("223caf13-9849-4e94-8fb2-6d054b066786") });

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Categories_CategoryId",
                table: "Attributes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
