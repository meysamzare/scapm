using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkbookId",
                table: "sm.Exams",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Workbook",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Workbook",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Workbook",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Workbook",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Workbooks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false),
                    IsShow = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workbooks", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 12, 28, 21, 42, 4, 322, DateTimeKind.Local).AddTicks(1266), new Guid("772f5575-11c7-414f-90a6-a65c77f576d6") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.Exams_WorkbookId",
                table: "sm.Exams",
                column: "WorkbookId");

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_Workbooks_WorkbookId",
                table: "sm.Exams",
                column: "WorkbookId",
                principalTable: "Workbooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_Workbooks_WorkbookId",
                table: "sm.Exams");

            migrationBuilder.DropTable(
                name: "Workbooks");

            migrationBuilder.DropIndex(
                name: "IX_sm.Exams_WorkbookId",
                table: "sm.Exams");

            migrationBuilder.DropColumn(
                name: "WorkbookId",
                table: "sm.Exams");

            migrationBuilder.DropColumn(
                name: "Add_Workbook",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Workbook",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Workbook",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Workbook",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 12, 19, 22, 7, 7, 304, DateTimeKind.Local).AddTicks(9228), new Guid("b50b6cb9-239e-41ea-a473-a0b4a104fd20") });
        }
    }
}
