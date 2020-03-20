using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_ClassBook",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_ClassBook",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_ClassBook",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_ClassBook",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ClassBooks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>(nullable: false),
                    InsTituteId = table.Column<int>(nullable: false),
                    YeareducationId = table.Column<int>(nullable: false),
                    GradeId = table.Column<int>(nullable: false),
                    ClassId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ExamId = table.Column<int>(nullable: true),
                    RegisterType = table.Column<int>(nullable: false),
                    RegisterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassBooks_sm.Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "sm.Class",
                        principalColumn: "Cls.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassBooks_sm.Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "sm.Course",
                        principalColumn: "Crs.Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassBooks_sm.Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "sm.Grade",
                        principalColumn: "Grd.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassBooks_sm.insTitute_InsTituteId",
                        column: x => x.InsTituteId,
                        principalTable: "sm.insTitute",
                        principalColumn: "ins.AutoNum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassBooks_sm.Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "sm.Student",
                        principalColumn: "Std.Autonum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassBooks_sm.Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "sm.Teacher",
                        principalColumn: "Tch.Autonum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassBooks_sm.Yeareducation_YeareducationId",
                        column: x => x.YeareducationId,
                        principalTable: "sm.Yeareducation",
                        principalColumn: "edu.YeareduCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 22, 14, 11, 7, 488, DateTimeKind.Local).AddTicks(5711), new Guid("ecbe6715-c429-4d36-a4bd-bd62fed6891c") });

            migrationBuilder.CreateIndex(
                name: "IX_ClassBooks_ClassId",
                table: "ClassBooks",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBooks_CourseId",
                table: "ClassBooks",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBooks_GradeId",
                table: "ClassBooks",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBooks_InsTituteId",
                table: "ClassBooks",
                column: "InsTituteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBooks_StudentId",
                table: "ClassBooks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBooks_TeacherId",
                table: "ClassBooks",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBooks_YeareducationId",
                table: "ClassBooks",
                column: "YeareducationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassBooks");

            migrationBuilder.DropColumn(
                name: "Add_ClassBook",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_ClassBook",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_ClassBook",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_ClassBook",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 19, 21, 51, 33, 327, DateTimeKind.Local).AddTicks(218), new Guid("687a5eb9-aae3-4125-8b65-00b052d3e864") });
        }
    }
}
