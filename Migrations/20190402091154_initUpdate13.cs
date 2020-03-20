using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sm.ExamTyp",
                columns: table => new
                {
                    Extypid = table.Column<int>(name: "Extyp.id", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExtypName = table.Column<string>(name: "Extyp.Name", nullable: true),
                    ExtypDesc = table.Column<string>(name: "Extyp.Desc", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.ExamTyp", x => x.Extypid);
                });

            migrationBuilder.CreateTable(
                name: "sm.Questions",
                columns: table => new
                {
                    Queid = table.Column<int>(name: "Que.id", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QueName = table.Column<string>(name: "Que.Name", nullable: true),
                    QueTyp = table.Column<int>(name: "Que.Typ", nullable: false),
                    QueQuestion = table.Column<string>(name: "Que.Question", nullable: true),
                    QueCourseid = table.Column<int>(name: "Que.Courseid", nullable: false),
                    QueGradeid = table.Column<int>(name: "Que.Gradeid", nullable: false),
                    QueSourceCreation = table.Column<string>(name: "Que.SourceCreation", nullable: true),
                    QueSource = table.Column<string>(name: "Que.Source", nullable: true),
                    QueMark = table.Column<double>(name: "Que.Mark", nullable: false),
                    QueDefact = table.Column<int>(name: "Que.Defact", nullable: false),
                    QueAnswer = table.Column<string>(name: "Que.Answer", nullable: true),
                    QueDescription = table.Column<string>(name: "Que.Description", nullable: true),
                    QueDesc = table.Column<string>(name: "Que.Desc", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.Questions", x => x.Queid);
                    table.ForeignKey(
                        name: "FK_sm.Questions_sm.Course_Que.Courseid",
                        column: x => x.QueCourseid,
                        principalTable: "sm.Course",
                        principalColumn: "Crs.Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.Questions_sm.Grade_Que.Gradeid",
                        column: x => x.QueGradeid,
                        principalTable: "sm.Grade",
                        principalColumn: "Grd.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sm.Exams",
                columns: table => new
                {
                    Exmid = table.Column<int>(name: "Exm.id", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Exname = table.Column<string>(name: "Ex.name", nullable: true),
                    ExDate = table.Column<DateTime>(name: "Ex.Date", nullable: false),
                    ExAmauntQuestion = table.Column<int>(name: "Ex.AmauntQuestion", nullable: false),
                    ExSource = table.Column<string>(name: "Ex.Source", nullable: true),
                    ExTopScore = table.Column<int>(name: "Ex.TopScore", nullable: false),
                    ExExamTyp = table.Column<int>(name: "Ex.ExamTyp", nullable: false),
                    ExGrade = table.Column<int>(name: "Ex.Grade", nullable: false),
                    ExClass = table.Column<int>(name: "Ex.Class", nullable: false),
                    ExTeacher = table.Column<int>(name: "Ex.Teacher", nullable: false),
                    ExOrder = table.Column<int>(name: "Ex.Order", nullable: false),
                    ExYear = table.Column<int>(name: "Ex.Year", nullable: false),
                    ExCourse = table.Column<int>(name: "Ex.Course", nullable: false),
                    ExTime = table.Column<int>(name: "Ex.Time", nullable: false),
                    ExResult = table.Column<bool>(name: "Ex.Result", nullable: false),
                    ExResultDate = table.Column<DateTime>(name: "Ex.ResultDate", nullable: false),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.Exams", x => x.Exmid);
                    table.ForeignKey(
                        name: "FK_sm.Exams_sm.Class_Ex.Class",
                        column: x => x.ExClass,
                        principalTable: "sm.Class",
                        principalColumn: "Cls.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.Exams_sm.Course_Ex.Course",
                        column: x => x.ExCourse,
                        principalTable: "sm.Course",
                        principalColumn: "Crs.Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.Exams_sm.ExamTyp_Ex.ExamTyp",
                        column: x => x.ExExamTyp,
                        principalTable: "sm.ExamTyp",
                        principalColumn: "Extyp.id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sm.Exams_sm.Grade_Ex.Grade",
                        column: x => x.ExGrade,
                        principalTable: "sm.Grade",
                        principalColumn: "Grd.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.Exams_sm.Exams_ParentId",
                        column: x => x.ParentId,
                        principalTable: "sm.Exams",
                        principalColumn: "Exm.id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.Exams_sm.Teacher_Ex.Teacher",
                        column: x => x.ExTeacher,
                        principalTable: "sm.Teacher",
                        principalColumn: "Tch.Autonum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.Exams_sm.Yeareducation_Ex.Year",
                        column: x => x.ExYear,
                        principalTable: "sm.Yeareducation",
                        principalColumn: "edu.YeareduCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sm.QuestionOptions",
                columns: table => new
                {
                    QueOpid = table.Column<int>(name: "QueOp.id", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QueOpname = table.Column<string>(name: "QueOp.name", nullable: true),
                    QueOpisAnswer = table.Column<bool>(name: "QueOp.isAnswer", nullable: false),
                    QueOpQuestionid = table.Column<int>(name: "QueOp.Questionid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.QuestionOptions", x => x.QueOpid);
                    table.ForeignKey(
                        name: "FK_sm.QuestionOptions_sm.Questions_QueOp.Questionid",
                        column: x => x.QueOpQuestionid,
                        principalTable: "sm.Questions",
                        principalColumn: "Que.id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sm.ExamScore",
                columns: table => new
                {
                    Exscid = table.Column<int>(name: "Exsc.id", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExscExamid = table.Column<int>(name: "Exsc.Examid", nullable: false),
                    ExscStudentid = table.Column<int>(name: "Exsc.Studentid", nullable: false),
                    ExScore = table.Column<double>(name: "Ex.Score", nullable: false),
                    ExTopScore = table.Column<int>(name: "Ex.TopScore", nullable: false),
                    ExAmauntQuestion = table.Column<int>(name: "Ex.AmauntQuestion", nullable: false),
                    ExCorrectAnswer = table.Column<int>(name: "Ex.CorrectAnswer", nullable: false),
                    ExWrongAnswer = table.Column<int>(name: "Ex.WrongAnswer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.ExamScore", x => x.Exscid);
                    table.ForeignKey(
                        name: "FK_sm.ExamScore_sm.Exams_Exsc.Examid",
                        column: x => x.ExscExamid,
                        principalTable: "sm.Exams",
                        principalColumn: "Exm.id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sm.ExamScore_sm.Student_Exsc.Studentid",
                        column: x => x.ExscStudentid,
                        principalTable: "sm.Student",
                        principalColumn: "Std.Autonum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 2, 13, 41, 54, 200, DateTimeKind.Local).AddTicks(1480), new Guid("f5fabf16-89fa-44af-99a2-ef5b0486abc6") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.Exams_Ex.Class",
                table: "sm.Exams",
                column: "Ex.Class");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Exams_Ex.Course",
                table: "sm.Exams",
                column: "Ex.Course");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Exams_Ex.ExamTyp",
                table: "sm.Exams",
                column: "Ex.ExamTyp");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Exams_Ex.Grade",
                table: "sm.Exams",
                column: "Ex.Grade");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Exams_ParentId",
                table: "sm.Exams",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Exams_Ex.Teacher",
                table: "sm.Exams",
                column: "Ex.Teacher");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Exams_Ex.Year",
                table: "sm.Exams",
                column: "Ex.Year");

            migrationBuilder.CreateIndex(
                name: "IX_sm.ExamScore_Exsc.Examid",
                table: "sm.ExamScore",
                column: "Exsc.Examid");

            migrationBuilder.CreateIndex(
                name: "IX_sm.ExamScore_Exsc.Studentid",
                table: "sm.ExamScore",
                column: "Exsc.Studentid");

            migrationBuilder.CreateIndex(
                name: "IX_sm.QuestionOptions_QueOp.Questionid",
                table: "sm.QuestionOptions",
                column: "QueOp.Questionid");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Questions_Que.Courseid",
                table: "sm.Questions",
                column: "Que.Courseid");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Questions_Que.Gradeid",
                table: "sm.Questions",
                column: "Que.Gradeid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sm.ExamScore");

            migrationBuilder.DropTable(
                name: "sm.QuestionOptions");

            migrationBuilder.DropTable(
                name: "sm.Exams");

            migrationBuilder.DropTable(
                name: "sm.Questions");

            migrationBuilder.DropTable(
                name: "sm.ExamTyp");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 1, 12, 31, 19, 138, DateTimeKind.Local).AddTicks(5213), new Guid("1ba1bbc1-ecb5-4832-ae9f-79e06f537931") });
        }
    }
}
