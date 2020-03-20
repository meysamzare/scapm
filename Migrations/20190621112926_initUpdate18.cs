using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_WorkBook_Details",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StdId = table.Column<int>(nullable: false),
                    StdName = table.Column<string>(nullable: true),
                    ColRow = table.Column<int>(nullable: false),
                    ColCourseName = table.Column<string>(nullable: true),
                    ColExam1 = table.Column<string>(nullable: true),
                    ColExam2 = table.Column<string>(nullable: true),
                    ColExam3 = table.Column<string>(nullable: true),
                    ColExam4 = table.Column<string>(nullable: true),
                    ColExam12 = table.Column<string>(nullable: true),
                    ColExam34 = table.Column<string>(nullable: true),
                    ColSumExam = table.Column<string>(nullable: true),
                    ColYearExam = table.Column<string>(nullable: true),
                    ColMinInClass = table.Column<string>(nullable: true),
                    ColMaxInClass = table.Column<string>(nullable: true),
                    ColAvgExam = table.Column<string>(nullable: true),
                    ColExamPercent = table.Column<string>(nullable: true),
                    ColScoreInClass = table.Column<string>(nullable: true),
                    ColScoreInGrade = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_WorkBook_Details", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_WorkBook_Header",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StdId = table.Column<int>(nullable: false),
                    StdName = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    enzebat1 = table.Column<string>(nullable: true),
                    enzebat2 = table.Column<string>(nullable: true),
                    jame1 = table.Column<string>(nullable: true),
                    jame2 = table.Column<string>(nullable: true),
                    jamekol = table.Column<string>(nullable: true),
                    nobat1 = table.Column<string>(nullable: true),
                    nobat2 = table.Column<string>(nullable: true),
                    moadelkol = table.Column<string>(nullable: true),
                    rotbeclass = table.Column<string>(nullable: true),
                    rotbepayeh = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_WorkBook_Header", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 21, 15, 59, 25, 173, DateTimeKind.Local).AddTicks(911), new Guid("c05de872-4dc5-4f93-ab15-56cdf0f690b0") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_WorkBook_Details");

            migrationBuilder.DropTable(
                name: "Tbl_WorkBook_Header");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 18, 21, 7, 7, 716, DateTimeKind.Local).AddTicks(2831), new Guid("ef2c0bf1-611b-428a-bb46-f44ae97c2270") });
        }
    }
}
