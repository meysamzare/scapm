using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sm.EducationTable",
                columns: table => new
                {
                    eduAutonum = table.Column<int>(name: "edu.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    eduName = table.Column<string>(name: "edu.Name", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.EducationTable", x => x.eduAutonum);
                });

            migrationBuilder.CreateTable(
                name: "sm.InsuranceTable",
                columns: table => new
                {
                    insAutonum = table.Column<int>(name: "ins.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    insName = table.Column<string>(name: "ins.Name", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.InsuranceTable", x => x.insAutonum);
                });

            migrationBuilder.CreateTable(
                name: "sm.OrgChart",
                columns: table => new
                {
                    OrgAutonum = table.Column<int>(name: "Org.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrgCode = table.Column<int>(name: "Org.Code", nullable: false),
                    OrgName = table.Column<string>(name: "Org.Name", nullable: true),
                    OrgParentCode = table.Column<int>(name: "Org.ParentCode", nullable: true),
                    OrgOrder = table.Column<int>(name: "Org.Order", nullable: false),
                    OrgDesc = table.Column<string>(name: "Org.Desc", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.OrgChart", x => x.OrgAutonum);
                    table.ForeignKey(
                        name: "FK_sm.OrgChart_sm.OrgChart_Org.ParentCode",
                        column: x => x.OrgParentCode,
                        principalTable: "sm.OrgChart",
                        principalColumn: "Org.Autonum",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sm.Salary",
                columns: table => new
                {
                    salAutonum = table.Column<int>(name: "sal.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    salName = table.Column<string>(name: "sal.Name", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.Salary", x => x.salAutonum);
                });

            migrationBuilder.CreateTable(
                name: "sm.Student",
                columns: table => new
                {
                    StdAutonum = table.Column<int>(name: "Std.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StdCode = table.Column<int>(name: "Std.Code", nullable: false),
                    StdOrgCode = table.Column<string>(name: "Std.OrgCode", nullable: true),
                    StdName = table.Column<string>(name: "Std.Name", nullable: true),
                    StdLastName = table.Column<string>(name: "Std.LastName", nullable: true),
                    StdFatherName = table.Column<string>(name: "Std.FatherName", nullable: true),
                    StdIdNumber = table.Column<string>(name: "Std.IdNumber", nullable: true),
                    StdIdNumber2 = table.Column<string>(name: "Std.IdNumber2", nullable: true),
                    StdBirthDate = table.Column<DateTime>(name: "Std.BirthDate", nullable: false),
                    StdIdCardSerial = table.Column<string>(name: "Std.IdCardSerial", nullable: true),
                    StdBirthLocation = table.Column<string>(name: "Std.BirthLocation", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.Student", x => x.StdAutonum);
                });

            migrationBuilder.CreateTable(
                name: "sm.TimeandDays",
                columns: table => new
                {
                    TdAutonum = table.Column<int>(name: "Td.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TdName = table.Column<string>(name: "Td.Name", nullable: true),
                    Tdsat = table.Column<bool>(name: "Td.sat", nullable: false),
                    Tdsun = table.Column<bool>(name: "Td.sun", nullable: false),
                    Tdmon = table.Column<bool>(name: "Td.mon", nullable: false),
                    Tdtue = table.Column<bool>(name: "Td.tue", nullable: false),
                    Tdwed = table.Column<bool>(name: "Td.wed", nullable: false),
                    Tdthr = table.Column<bool>(name: "Td.thr", nullable: false),
                    Tdfri = table.Column<bool>(name: "Td.fri", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.TimeandDays", x => x.TdAutonum);
                });

            migrationBuilder.CreateTable(
                name: "sm.OrgPerson",
                columns: table => new
                {
                    OrgprsAutonum = table.Column<int>(name: "Orgprs.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrgprsCode = table.Column<int>(name: "Orgprs.Code", nullable: false),
                    OrgprsName = table.Column<string>(name: "Orgprs.Name", nullable: true),
                    OrgprsLastName = table.Column<string>(name: "Orgprs.LastName", nullable: true),
                    OrgprsFatherName = table.Column<string>(name: "Orgprs.FatherName", nullable: true),
                    Orgprssex = table.Column<int>(name: "Orgprs.sex", nullable: false),
                    Orgprsidnum = table.Column<string>(name: "Orgprs.idnum", nullable: true),
                    OrgprsBirthDate = table.Column<DateTime>(name: "Orgprs.BirthDate", nullable: false),
                    Orgprsidnumber = table.Column<string>(name: "Orgprs.idnumber", nullable: true),
                    Orgprsidserial = table.Column<string>(name: "Orgprs.idserial", nullable: true),
                    OrgprsMarrage = table.Column<bool>(name: "Orgprs.Marrage", nullable: false),
                    OrgprsChild = table.Column<int>(name: "Orgprs.Child", nullable: false),
                    OrgprsinsuranceId = table.Column<string>(name: "Orgprs.insuranceId", nullable: true),
                    OrgprsType = table.Column<string>(name: "Orgprs.Type", nullable: true),
                    OrgprsTypeYear = table.Column<string>(name: "Orgprs.TypeYear", nullable: true),
                    OrgprsAddress = table.Column<string>(name: "Orgprs.Address", nullable: true),
                    OrgprsTell = table.Column<string>(name: "Orgprs.Tell", nullable: true),
                    OrgprsPhone = table.Column<string>(name: "Orgprs.Phone", nullable: true),
                    OrgprsEmail = table.Column<string>(name: "Orgprs.Email", nullable: true),
                    OrgprsChartCode = table.Column<int>(name: "Orgprs.ChartCode", nullable: false),
                    OrgprsSalary = table.Column<int>(name: "Orgprs.Salary", nullable: false),
                    Orgprsedu = table.Column<int>(name: "Orgprs.edu", nullable: false),
                    OrgprsinsuranceTyp = table.Column<int>(name: "Orgprs.insuranceTyp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.OrgPerson", x => x.OrgprsAutonum);
                    table.ForeignKey(
                        name: "FK_sm.OrgPerson_sm.EducationTable_Orgprs.edu",
                        column: x => x.Orgprsedu,
                        principalTable: "sm.EducationTable",
                        principalColumn: "edu.Autonum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sm.OrgPerson_sm.InsuranceTable_Orgprs.insuranceTyp",
                        column: x => x.OrgprsinsuranceTyp,
                        principalTable: "sm.InsuranceTable",
                        principalColumn: "ins.Autonum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sm.OrgPerson_sm.OrgChart_Orgprs.ChartCode",
                        column: x => x.OrgprsChartCode,
                        principalTable: "sm.OrgChart",
                        principalColumn: "Org.Autonum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sm.OrgPerson_sm.Salary_Orgprs.Salary",
                        column: x => x.OrgprsSalary,
                        principalTable: "sm.Salary",
                        principalColumn: "sal.Autonum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sm.StdClassMng",
                columns: table => new
                {
                    StdClassMngId = table.Column<int>(name: "StdClassMng.Id", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StdCode = table.Column<int>(name: "Std.Code", nullable: false),
                    StdClassid = table.Column<int>(name: "Std.Classid", nullable: false),
                    StdGradeid = table.Column<int>(name: "Std.Gradeid", nullable: false),
                    StdYearId = table.Column<int>(name: "Std.YearId", nullable: false),
                    Stdinsid = table.Column<int>(name: "Std.insid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.StdClassMng", x => x.StdClassMngId);
                    table.ForeignKey(
                        name: "FK_sm.StdClassMng_sm.Class_Std.Classid",
                        column: x => x.StdClassid,
                        principalTable: "sm.Class",
                        principalColumn: "Cls.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.StdClassMng_sm.Grade_Std.Gradeid",
                        column: x => x.StdGradeid,
                        principalTable: "sm.Grade",
                        principalColumn: "Grd.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.StdClassMng_sm.insTitute_Std.insid",
                        column: x => x.Stdinsid,
                        principalTable: "sm.insTitute",
                        principalColumn: "ins.AutoNum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.StdClassMng_sm.Student_Std.Code",
                        column: x => x.StdCode,
                        principalTable: "sm.Student",
                        principalColumn: "Std.Autonum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.StdClassMng_sm.Yeareducation_Std.YearId",
                        column: x => x.StdYearId,
                        principalTable: "sm.Yeareducation",
                        principalColumn: "edu.YeareduCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sm.StudentInfo",
                columns: table => new
                {
                    StdinfoAutonum = table.Column<int>(name: "Stdinfo.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StdinfoFatherName = table.Column<string>(name: "Stdinfo.FatherName", nullable: true),
                    StdinfoFatherEdu = table.Column<string>(name: "Stdinfo.FatherEdu", nullable: true),
                    StdinfoFatherJob = table.Column<string>(name: "Stdinfo.FatherJob", nullable: true),
                    StdinfoFatherJobPhone = table.Column<string>(name: "Stdinfo.FatherJobPhone", nullable: true),
                    StdinfoFatherPhone = table.Column<string>(name: "Stdinfo.FatherPhone", nullable: true),
                    StdinfoFatherJobAddress = table.Column<string>(name: "Stdinfo.FatherJobAddress", nullable: true),
                    StdinfoMomName = table.Column<string>(name: "Stdinfo.MomName", nullable: true),
                    StdinfoMomEdu = table.Column<string>(name: "Stdinfo.MomEdu", nullable: true),
                    StdinfoMomJob = table.Column<string>(name: "Stdinfo.MomJob", nullable: true),
                    StdinfoMomJobPhone = table.Column<string>(name: "Stdinfo.MomJobPhone", nullable: true),
                    StdinfoMomPhone = table.Column<string>(name: "Stdinfo.MomPhone", nullable: true),
                    StdinfoMomJobAddress = table.Column<string>(name: "Stdinfo.MomJobAddress", nullable: true),
                    StdinfoHomeAddress = table.Column<string>(name: "Stdinfo.HomeAddress", nullable: true),
                    StdinfoHomePhone = table.Column<string>(name: "Stdinfo.HomePhone", nullable: true),
                    StdinfoFamilyState = table.Column<string>(name: "Stdinfo.FamilyState", nullable: true),
                    StdinfoReligion = table.Column<string>(name: "Stdinfo.Religion", nullable: true),
                    StdinfoSocialNet = table.Column<string>(name: "Stdinfo.SocialNet", nullable: true),
                    StdinfoEmail = table.Column<string>(name: "Stdinfo.Email", nullable: true),
                    StdinfoCode = table.Column<int>(name: "Stdinfo.Code", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.StudentInfo", x => x.StdinfoAutonum);
                    table.ForeignKey(
                        name: "FK_sm.StudentInfo_sm.Student_Stdinfo.Code",
                        column: x => x.StdinfoCode,
                        principalTable: "sm.Student",
                        principalColumn: "Std.Autonum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sm.Teacher",
                columns: table => new
                {
                    TchAutonum = table.Column<int>(name: "Tch.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TchId = table.Column<int>(name: "Tch.Id", nullable: false),
                    TchName = table.Column<int>(name: "Tch.Name", nullable: false),
                    TchPrsCode = table.Column<int>(name: "Tch.PrsCode", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.Teacher", x => x.TchAutonum);
                    table.ForeignKey(
                        name: "FK_sm.Teacher_sm.OrgPerson_Tch.PrsCode",
                        column: x => x.TchPrsCode,
                        principalTable: "sm.OrgPerson",
                        principalColumn: "Orgprs.Autonum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sm.Course",
                columns: table => new
                {
                    CrsCode = table.Column<int>(name: "Crs.Code", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CrsName = table.Column<string>(name: "Crs.Name", nullable: true),
                    CrsGradeCode = table.Column<int>(name: "Crs.GradeCode", nullable: false),
                    CrsCourseMix = table.Column<int>(name: "Crs.CourseMix", nullable: false),
                    CrsCourseOrder = table.Column<int>(name: "Crs.CourseOrder", nullable: false),
                    CrsCourseOrder2 = table.Column<int>(name: "Crs.CourseOrder2", nullable: false),
                    CrsTeacherCode = table.Column<int>(name: "Crs.TeacherCode", nullable: false),
                    CrsTeachTime = table.Column<int>(name: "Crs.TeachTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.Course", x => x.CrsCode);
                    table.ForeignKey(
                        name: "FK_sm.Course_sm.Grade_Crs.GradeCode",
                        column: x => x.CrsGradeCode,
                        principalTable: "sm.Grade",
                        principalColumn: "Grd.Autonumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sm.Course_sm.Teacher_Crs.TeacherCode",
                        column: x => x.CrsTeacherCode,
                        principalTable: "sm.Teacher",
                        principalColumn: "Tch.Autonum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sm.TimeSchedule",
                columns: table => new
                {
                    TschAutonum = table.Column<int>(name: "Tsch.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TschName = table.Column<string>(name: "Tsch.Name", nullable: true),
                    TschCourseCode = table.Column<int>(name: "Tsch.CourseCode", nullable: false),
                    TschTeacherCode = table.Column<int>(name: "Tsch.TeacherCode", nullable: false),
                    TschTimeStart = table.Column<TimeSpan>(name: "Tsch.TimeStart", nullable: false),
                    TschTimeEnd = table.Column<TimeSpan>(name: "Tsch.TimeEnd", nullable: false),
                    TschDaysId = table.Column<int>(name: "Tsch.DaysId", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.TimeSchedule", x => x.TschAutonum);
                    table.ForeignKey(
                        name: "FK_sm.TimeSchedule_sm.Course_Tsch.CourseCode",
                        column: x => x.TschCourseCode,
                        principalTable: "sm.Course",
                        principalColumn: "Crs.Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sm.TimeSchedule_sm.Teacher_Tsch.TeacherCode",
                        column: x => x.TschTeacherCode,
                        principalTable: "sm.Teacher",
                        principalColumn: "Tch.Autonum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sm.TimeSchedule_sm.TimeandDays_Tsch.DaysId",
                        column: x => x.TschDaysId,
                        principalTable: "sm.TimeandDays",
                        principalColumn: "Td.Autonum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 28, 11, 46, 39, 349, DateTimeKind.Local).AddTicks(6070), new Guid("ebf29eda-fae2-49ab-bad9-8db905212214") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.Course_Crs.GradeCode",
                table: "sm.Course",
                column: "Crs.GradeCode");

            migrationBuilder.CreateIndex(
                name: "IX_sm.Course_Crs.TeacherCode",
                table: "sm.Course",
                column: "Crs.TeacherCode");

            migrationBuilder.CreateIndex(
                name: "IX_sm.OrgChart_Org.ParentCode",
                table: "sm.OrgChart",
                column: "Org.ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_sm.OrgPerson_Orgprs.edu",
                table: "sm.OrgPerson",
                column: "Orgprs.edu");

            migrationBuilder.CreateIndex(
                name: "IX_sm.OrgPerson_Orgprs.insuranceTyp",
                table: "sm.OrgPerson",
                column: "Orgprs.insuranceTyp");

            migrationBuilder.CreateIndex(
                name: "IX_sm.OrgPerson_Orgprs.ChartCode",
                table: "sm.OrgPerson",
                column: "Orgprs.ChartCode");

            migrationBuilder.CreateIndex(
                name: "IX_sm.OrgPerson_Orgprs.Salary",
                table: "sm.OrgPerson",
                column: "Orgprs.Salary");

            migrationBuilder.CreateIndex(
                name: "IX_sm.StdClassMng_Std.Classid",
                table: "sm.StdClassMng",
                column: "Std.Classid");

            migrationBuilder.CreateIndex(
                name: "IX_sm.StdClassMng_Std.Gradeid",
                table: "sm.StdClassMng",
                column: "Std.Gradeid");

            migrationBuilder.CreateIndex(
                name: "IX_sm.StdClassMng_Std.insid",
                table: "sm.StdClassMng",
                column: "Std.insid");

            migrationBuilder.CreateIndex(
                name: "IX_sm.StdClassMng_Std.Code",
                table: "sm.StdClassMng",
                column: "Std.Code");

            migrationBuilder.CreateIndex(
                name: "IX_sm.StdClassMng_Std.YearId",
                table: "sm.StdClassMng",
                column: "Std.YearId");

            migrationBuilder.CreateIndex(
                name: "IX_sm.StudentInfo_Stdinfo.Code",
                table: "sm.StudentInfo",
                column: "Stdinfo.Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sm.Teacher_Tch.PrsCode",
                table: "sm.Teacher",
                column: "Tch.PrsCode");

            migrationBuilder.CreateIndex(
                name: "IX_sm.TimeSchedule_Tsch.CourseCode",
                table: "sm.TimeSchedule",
                column: "Tsch.CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_sm.TimeSchedule_Tsch.TeacherCode",
                table: "sm.TimeSchedule",
                column: "Tsch.TeacherCode");

            migrationBuilder.CreateIndex(
                name: "IX_sm.TimeSchedule_Tsch.DaysId",
                table: "sm.TimeSchedule",
                column: "Tsch.DaysId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sm.StdClassMng");

            migrationBuilder.DropTable(
                name: "sm.StudentInfo");

            migrationBuilder.DropTable(
                name: "sm.TimeSchedule");

            migrationBuilder.DropTable(
                name: "sm.Student");

            migrationBuilder.DropTable(
                name: "sm.Course");

            migrationBuilder.DropTable(
                name: "sm.TimeandDays");

            migrationBuilder.DropTable(
                name: "sm.Teacher");

            migrationBuilder.DropTable(
                name: "sm.OrgPerson");

            migrationBuilder.DropTable(
                name: "sm.EducationTable");

            migrationBuilder.DropTable(
                name: "sm.InsuranceTable");

            migrationBuilder.DropTable(
                name: "sm.OrgChart");

            migrationBuilder.DropTable(
                name: "sm.Salary");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 25, 22, 19, 7, 829, DateTimeKind.Local).AddTicks(9414), new Guid("25eba012-5340-4d13-8079-8f7366021018") });
        }
    }
}
