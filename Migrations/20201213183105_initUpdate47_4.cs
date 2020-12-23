using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate47_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Classes_Ex.Class",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamTypes_Ex.ExamTyp",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Teachers_Ex.Teacher",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Yeareducations_Ex.Year",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamScores_DescriptiveScores_DescriptiveScoreId",
                table: "ExamScores");

            migrationBuilder.DropIndex(
                name: "IX_ExamScores_DescriptiveScoreId",
                table: "ExamScores");

            migrationBuilder.DropColumn(
                name: "DescriptiveScoreId",
                table: "ExamScores");

            migrationBuilder.DropColumn(
                name: "Ex.AmauntQuestion",
                table: "ExamScores");

            migrationBuilder.DropColumn(
                name: "Ex.TopScore",
                table: "ExamScores");

            migrationBuilder.RenameColumn(
                name: "Ex.Year",
                table: "Exams",
                newName: "YeareducationId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_Ex.Year",
                table: "Exams",
                newName: "IX_Exams_YeareducationId");

            migrationBuilder.AlterColumn<double>(
                name: "Ex.TopScore",
                table: "Exams",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Ex.Teacher",
                table: "Exams",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Ex.ExamTyp",
                table: "Exams",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Ex.Class",
                table: "Exams",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "YeareducationId",
                table: "Exams",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "OnlineExamId",
                table: "Exams",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 13, 22, 1, 3, 781, DateTimeKind.Local).AddTicks(3206), new Guid("2de07199-9680-4b83-91b4-981066f40d08") });

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Classes_Ex.Class",
                table: "Exams",
                column: "Ex.Class",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamTypes_Ex.ExamTyp",
                table: "Exams",
                column: "Ex.ExamTyp",
                principalTable: "ExamTypes",
                principalColumn: "Extyp.id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Teachers_Ex.Teacher",
                table: "Exams",
                column: "Ex.Teacher",
                principalTable: "Teachers",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Yeareducations_YeareducationId",
                table: "Exams",
                column: "YeareducationId",
                principalTable: "Yeareducations",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Classes_Ex.Class",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamTypes_Ex.ExamTyp",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Teachers_Ex.Teacher",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Yeareducations_YeareducationId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "OnlineExamId",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "YeareducationId",
                table: "Exams",
                newName: "Ex.Year");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_YeareducationId",
                table: "Exams",
                newName: "IX_Exams_Ex.Year");

            migrationBuilder.AddColumn<int>(
                name: "DescriptiveScoreId",
                table: "ExamScores",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ex.AmauntQuestion",
                table: "ExamScores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ex.TopScore",
                table: "ExamScores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Ex.TopScore",
                table: "Exams",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Ex.Teacher",
                table: "Exams",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ex.ExamTyp",
                table: "Exams",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ex.Class",
                table: "Exams",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ex.Year",
                table: "Exams",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 6, 20, 49, 26, 592, DateTimeKind.Local).AddTicks(3297), new Guid("dd992f90-d2e2-4444-9d5c-9324c667f9ff") });

            migrationBuilder.CreateIndex(
                name: "IX_ExamScores_DescriptiveScoreId",
                table: "ExamScores",
                column: "DescriptiveScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Classes_Ex.Class",
                table: "Exams",
                column: "Ex.Class",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamTypes_Ex.ExamTyp",
                table: "Exams",
                column: "Ex.ExamTyp",
                principalTable: "ExamTypes",
                principalColumn: "Extyp.id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Teachers_Ex.Teacher",
                table: "Exams",
                column: "Ex.Teacher",
                principalTable: "Teachers",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Yeareducations_Ex.Year",
                table: "Exams",
                column: "Ex.Year",
                principalTable: "Yeareducations",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamScores_DescriptiveScores_DescriptiveScoreId",
                table: "ExamScores",
                column: "DescriptiveScoreId",
                principalTable: "DescriptiveScores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
