using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_ScoreThemplate",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_StudentScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_ScoreThemplate",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_StudentScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_ScoreThemplate",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_StudentScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_ScoreThemplate",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_StudentScore",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ScoreThemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreThemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    ExporterId = table.Column<int>(nullable: false),
                    ExporterName = table.Column<string>(nullable: true),
                    ExporterType = table.Column<int>(nullable: false),
                    StdClassMngId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentScores_sm.StdClassMng_StdClassMngId",
                        column: x => x.StdClassMngId,
                        principalTable: "sm.StdClassMng",
                        principalColumn: "StdClassMng.Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 7, 10, 1, 38, 443, DateTimeKind.Local).AddTicks(9991), new Guid("7aec0604-c70f-434b-823c-ffeac9401279") });

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_StdClassMngId",
                table: "StudentScores",
                column: "StdClassMngId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreThemplates");

            migrationBuilder.DropTable(
                name: "StudentScores");

            migrationBuilder.DropColumn(
                name: "Add_ScoreThemplate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_StudentScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_ScoreThemplate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_StudentScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_ScoreThemplate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_StudentScore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_ScoreThemplate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_StudentScore",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 1, 20, 2, 57, 969, DateTimeKind.Local).AddTicks(6177), new Guid("8afc2cfd-1e37-4741-b49c-a662828cd472") });
        }
    }
}
