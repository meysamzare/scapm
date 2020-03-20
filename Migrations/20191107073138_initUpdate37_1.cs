using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate37_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExporterId",
                table: "StudentScores");

            migrationBuilder.DropColumn(
                name: "ExporterName",
                table: "StudentScores");

            migrationBuilder.RenameColumn(
                name: "ExporterType",
                table: "StudentScores",
                newName: "TeacherId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "StudentScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 7, 11, 1, 37, 444, DateTimeKind.Local).AddTicks(1887), new Guid("4e7f5276-3e10-4791-8bb1-7db9cbc6985c") });

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_TeacherId",
                table: "StudentScores",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScores_sm.Teacher_TeacherId",
                table: "StudentScores",
                column: "TeacherId",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentScores_sm.Teacher_TeacherId",
                table: "StudentScores");

            migrationBuilder.DropIndex(
                name: "IX_StudentScores_TeacherId",
                table: "StudentScores");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "StudentScores");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "StudentScores",
                newName: "ExporterType");

            migrationBuilder.AddColumn<int>(
                name: "ExporterId",
                table: "StudentScores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExporterName",
                table: "StudentScores",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 7, 10, 1, 38, 443, DateTimeKind.Local).AddTicks(9991), new Guid("7aec0604-c70f-434b-823c-ffeac9401279") });
        }
    }
}
