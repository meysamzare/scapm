using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "View_StudentDailySchedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StudentDailySchedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StdClassMngId = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateExecute = table.Column<DateTime>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false, defaultValue: 1),
                    Type = table.Column<int>(nullable: false),
                    Volume = table.Column<string>(nullable: true),
                    FromTime = table.Column<string>(nullable: true),
                    ToTime = table.Column<string>(nullable: true),
                    StudentParentComment = table.Column<string>(nullable: true),
                    StudentParentCommentDate = table.Column<DateTime>(nullable: true),
                    ConsultantComment = table.Column<string>(nullable: true),
                    ConsultantCommentDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDailySchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDailySchedules_sm.Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "sm.Course",
                        principalColumn: "Crs.Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentDailySchedules_sm.StdClassMng_StdClassMngId",
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
                values: new object[] { new DateTime(2020, 9, 25, 15, 45, 16, 35, DateTimeKind.Local).AddTicks(9098), new Guid("975c5178-27f5-4684-8f27-0d94c1a8f8b2") });

            migrationBuilder.CreateIndex(
                name: "IX_StudentDailySchedules_CourseId",
                table: "StudentDailySchedules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDailySchedules_StdClassMngId",
                table: "StudentDailySchedules",
                column: "StdClassMngId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentDailySchedules");

            migrationBuilder.DropColumn(
                name: "View_StudentDailySchedule",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 9, 12, 23, 46, 55, 972, DateTimeKind.Local).AddTicks(6163), new Guid("38097ba1-7cac-4546-9d3a-27a9b1dd143c") });
        }
    }
}
