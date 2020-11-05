using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_sm.Questions_QuestionId",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_sm.Course_CourseId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_sm.ExamTyp_ExamTypeId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_sm.Grade_GradeId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_sm.Course_CourseId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_sm.Grade_GradeId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_sm.insTitute_InsTituteId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_sm.Student_StudentId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_sm.Teacher_TeacherId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_sm.Yeareducation_YeareducationId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_sm.Grade_Grd.Id",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Fin.Contract_Fin.ContractType_ContractTypeId",
                table: "Fin.Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_Fin.StdPayment_Fin.Contract_ContractId",
                table: "Fin.StdPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Fin.StdPayment_Fin.PaymentType_StdPayment.PaymentTyp",
                table: "Fin.StdPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Fin.StdPayment_sm.Student_StdPayment.Student",
                table: "Fin.StdPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Ind.Comment_Ind.Comment_ParentId",
                table: "Ind.Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Ind.Comment_Ind.Post_PostId",
                table: "Ind.Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Ind.MainSlideShow_Ind.Post_PostId",
                table: "Ind.MainSlideShow");

            migrationBuilder.DropForeignKey(
                name: "FK_Ind.Picture_Ind.PictureGallery_PictureGalleryId",
                table: "Ind.Picture");

            migrationBuilder.DropForeignKey(
                name: "FK_Ind.Schedule_Ind.Post_PostId",
                table: "Ind.Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationAgents_sm.Student_StudentId",
                table: "NotificationAgents");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_sm.Course_CourseId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_sm.Grade_GradeId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Course_sm.Grade_Crs.GradeCode",
                table: "sm.Course");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Course_sm.Teacher_Crs.TeacherCode",
                table: "sm.Course");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_Classes_Ex.Class",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_sm.Course_Ex.Course",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_sm.ExamTyp_Ex.ExamTyp",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_sm.Grade_Ex.Grade",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_sm.Exams_ParentId",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_sm.Teacher_Ex.Teacher",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_Workbooks_WorkbookId",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_sm.Yeareducation_Ex.Year",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.ExamScore_DescriptiveScores_DescriptiveScoreId",
                table: "sm.ExamScore");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.ExamScore_sm.Exams_Exsc.Examid",
                table: "sm.ExamScore");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.ExamScore_sm.Student_Exsc.Studentid",
                table: "sm.ExamScore");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Grade_sm.insTitute_Grd.insCode",
                table: "sm.Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Grade_sm.Yeareducation_Grd.eduyearCode",
                table: "sm.Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.insTitute_sm.insTitute_ins.Code",
                table: "sm.insTitute");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.OrgChart_sm.OrgChart_Org.ParentCode",
                table: "sm.OrgChart");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.OrgPerson_sm.EducationTable_Orgprs.edu",
                table: "sm.OrgPerson");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.OrgPerson_sm.InsuranceTable_Orgprs.insuranceTyp",
                table: "sm.OrgPerson");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.OrgPerson_sm.OrgChart_Orgprs.ChartCode",
                table: "sm.OrgPerson");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.OrgPerson_sm.Salary_Orgprs.Salary",
                table: "sm.OrgPerson");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.QuestionOptions_sm.Questions_QueOp.Questionid",
                table: "sm.QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Questions_sm.Course_Que.Courseid",
                table: "sm.Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Questions_sm.Grade_Que.Gradeid",
                table: "sm.Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_Classes_Std.Classid",
                table: "sm.StdClassMng");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_sm.Grade_Std.Gradeid",
                table: "sm.StdClassMng");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_sm.insTitute_Std.insid",
                table: "sm.StdClassMng");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_sm.Student_Std.Code",
                table: "sm.StdClassMng");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_StudentTypes_StudentTypeId",
                table: "sm.StdClassMng");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_sm.Yeareducation_Std.YearId",
                table: "sm.StdClassMng");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StudentInfo_sm.Student_Stdinfo.Code",
                table: "sm.StudentInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Teacher_sm.OrgPerson_Tch.PrsCode",
                table: "sm.Teacher");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.TicketConversation_sm.Ticket_TicketId",
                table: "sm.TicketConversation");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.TimeSchedule_sm.Course_Tsch.CourseCode",
                table: "sm.TimeSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_Tsch.TeacherCode",
                table: "sm.TimeSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_TeacherId1",
                table: "sm.TimeSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.TimeSchedule_sm.TimeandDays_Tsch.DaysId",
                table: "sm.TimeSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDailySchedules_sm.Course_CourseId",
                table: "StudentDailySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDailySchedules_sm.StdClassMng_StdClassMngId",
                table: "StudentDailySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentScores_sm.StdClassMng_StdClassMngId",
                table: "StudentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentScores_sm.Teacher_TeacherId",
                table: "StudentScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Yeareducation",
                table: "sm.Yeareducation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.TimeSchedule",
                table: "sm.TimeSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.TimeandDays",
                table: "sm.TimeandDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.TicketConversation",
                table: "sm.TicketConversation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Ticket",
                table: "sm.Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Teacher",
                table: "sm.Teacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.StudentInfo",
                table: "sm.StudentInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Student",
                table: "sm.Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.StdClassMng",
                table: "sm.StdClassMng");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Salary",
                table: "sm.Salary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Questions",
                table: "sm.Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.QuestionOptions",
                table: "sm.QuestionOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.OrgPerson",
                table: "sm.OrgPerson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.OrgChart",
                table: "sm.OrgChart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.InsuranceTable",
                table: "sm.InsuranceTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.insTitute",
                table: "sm.insTitute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Grade",
                table: "sm.Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.ExamTyp",
                table: "sm.ExamTyp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.ExamScore",
                table: "sm.ExamScore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Exams",
                table: "sm.Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.EducationTable",
                table: "sm.EducationTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Course",
                table: "sm.Course");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ind.Schedule",
                table: "Ind.Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ind.Post",
                table: "Ind.Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ind.PictureGallery",
                table: "Ind.PictureGallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ind.Picture",
                table: "Ind.Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ind.MainSlideShow",
                table: "Ind.MainSlideShow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ind.Comment",
                table: "Ind.Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ind.BestStudent",
                table: "Ind.BestStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ind.Advertising",
                table: "Ind.Advertising");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fin.StdPayment",
                table: "Fin.StdPayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fin.PaymentType",
                table: "Fin.PaymentType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fin.ContractType",
                table: "Fin.ContractType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fin.Contract",
                table: "Fin.Contract");

            migrationBuilder.RenameTable(
                name: "sm.Yeareducation",
                newName: "Yeareducations");

            migrationBuilder.RenameTable(
                name: "sm.TimeSchedule",
                newName: "TimeSchedules");

            migrationBuilder.RenameTable(
                name: "sm.TimeandDays",
                newName: "TimeandDays");

            migrationBuilder.RenameTable(
                name: "sm.TicketConversation",
                newName: "TicketConversations");

            migrationBuilder.RenameTable(
                name: "sm.Ticket",
                newName: "Tickets");

            migrationBuilder.RenameTable(
                name: "sm.Teacher",
                newName: "Teachers");

            migrationBuilder.RenameTable(
                name: "sm.StudentInfo",
                newName: "StudentInfos");

            migrationBuilder.RenameTable(
                name: "sm.Student",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "sm.StdClassMng",
                newName: "StdClassMngs");

            migrationBuilder.RenameTable(
                name: "sm.Salary",
                newName: "Salaries");

            migrationBuilder.RenameTable(
                name: "sm.Questions",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "sm.QuestionOptions",
                newName: "QuestionOptions");

            migrationBuilder.RenameTable(
                name: "sm.OrgPerson",
                newName: "OrgPeople");

            migrationBuilder.RenameTable(
                name: "sm.OrgChart",
                newName: "OrgCharts");

            migrationBuilder.RenameTable(
                name: "sm.InsuranceTable",
                newName: "Insurances");

            migrationBuilder.RenameTable(
                name: "sm.insTitute",
                newName: "InsTitutes");

            migrationBuilder.RenameTable(
                name: "sm.Grade",
                newName: "Grades");

            migrationBuilder.RenameTable(
                name: "sm.ExamTyp",
                newName: "ExamTypes");

            migrationBuilder.RenameTable(
                name: "sm.ExamScore",
                newName: "ExamScores");

            migrationBuilder.RenameTable(
                name: "sm.Exams",
                newName: "Exams");

            migrationBuilder.RenameTable(
                name: "sm.EducationTable",
                newName: "Educations");

            migrationBuilder.RenameTable(
                name: "sm.Course",
                newName: "Courses");

            migrationBuilder.RenameTable(
                name: "Ind.Schedule",
                newName: "Schedules");

            migrationBuilder.RenameTable(
                name: "Ind.Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "Ind.PictureGallery",
                newName: "PictureGalleries");

            migrationBuilder.RenameTable(
                name: "Ind.Picture",
                newName: "Pictures");

            migrationBuilder.RenameTable(
                name: "Ind.MainSlideShow",
                newName: "MainSlideShows");

            migrationBuilder.RenameTable(
                name: "Ind.Comment",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Ind.BestStudent",
                newName: "BestStudents");

            migrationBuilder.RenameTable(
                name: "Ind.Advertising",
                newName: "Advertisings");

            migrationBuilder.RenameTable(
                name: "Fin.StdPayment",
                newName: "StdPayments");

            migrationBuilder.RenameTable(
                name: "Fin.PaymentType",
                newName: "PaymentTypes");

            migrationBuilder.RenameTable(
                name: "Fin.ContractType",
                newName: "ContractTypes");

            migrationBuilder.RenameTable(
                name: "Fin.Contract",
                newName: "Contracts");

            migrationBuilder.RenameIndex(
                name: "IX_sm.TimeSchedule_Tsch.DaysId",
                table: "TimeSchedules",
                newName: "IX_TimeSchedules_Tsch.DaysId");

            migrationBuilder.RenameIndex(
                name: "IX_sm.TimeSchedule_TeacherId1",
                table: "TimeSchedules",
                newName: "IX_TimeSchedules_TeacherId1");

            migrationBuilder.RenameIndex(
                name: "IX_sm.TimeSchedule_Tsch.TeacherCode",
                table: "TimeSchedules",
                newName: "IX_TimeSchedules_Tsch.TeacherCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.TimeSchedule_Tsch.CourseCode",
                table: "TimeSchedules",
                newName: "IX_TimeSchedules_Tsch.CourseCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.TicketConversation_TicketId",
                table: "TicketConversations",
                newName: "IX_TicketConversations_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Teacher_Tch.PrsCode",
                table: "Teachers",
                newName: "IX_Teachers_Tch.PrsCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.StudentInfo_Stdinfo.Code",
                table: "StudentInfos",
                newName: "IX_StudentInfos_Stdinfo.Code");

            migrationBuilder.RenameIndex(
                name: "IX_sm.StdClassMng_Std.YearId",
                table: "StdClassMngs",
                newName: "IX_StdClassMngs_Std.YearId");

            migrationBuilder.RenameIndex(
                name: "IX_sm.StdClassMng_StudentTypeId",
                table: "StdClassMngs",
                newName: "IX_StdClassMngs_StudentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_sm.StdClassMng_Std.Code",
                table: "StdClassMngs",
                newName: "IX_StdClassMngs_Std.Code");

            migrationBuilder.RenameIndex(
                name: "IX_sm.StdClassMng_Std.insid",
                table: "StdClassMngs",
                newName: "IX_StdClassMngs_Std.insid");

            migrationBuilder.RenameIndex(
                name: "IX_sm.StdClassMng_Std.Gradeid",
                table: "StdClassMngs",
                newName: "IX_StdClassMngs_Std.Gradeid");

            migrationBuilder.RenameIndex(
                name: "IX_sm.StdClassMng_Std.Classid",
                table: "StdClassMngs",
                newName: "IX_StdClassMngs_Std.Classid");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Questions_Que.Gradeid",
                table: "Questions",
                newName: "IX_Questions_Que.Gradeid");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Questions_Que.Courseid",
                table: "Questions",
                newName: "IX_Questions_Que.Courseid");

            migrationBuilder.RenameIndex(
                name: "IX_sm.QuestionOptions_QueOp.Questionid",
                table: "QuestionOptions",
                newName: "IX_QuestionOptions_QueOp.Questionid");

            migrationBuilder.RenameIndex(
                name: "IX_sm.OrgPerson_Orgprs.Salary",
                table: "OrgPeople",
                newName: "IX_OrgPeople_Orgprs.Salary");

            migrationBuilder.RenameIndex(
                name: "IX_sm.OrgPerson_Orgprs.ChartCode",
                table: "OrgPeople",
                newName: "IX_OrgPeople_Orgprs.ChartCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.OrgPerson_Orgprs.insuranceTyp",
                table: "OrgPeople",
                newName: "IX_OrgPeople_Orgprs.insuranceTyp");

            migrationBuilder.RenameIndex(
                name: "IX_sm.OrgPerson_Orgprs.edu",
                table: "OrgPeople",
                newName: "IX_OrgPeople_Orgprs.edu");

            migrationBuilder.RenameIndex(
                name: "IX_sm.OrgChart_Org.ParentCode",
                table: "OrgCharts",
                newName: "IX_OrgCharts_Org.ParentCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.insTitute_ins.Code",
                table: "InsTitutes",
                newName: "IX_InsTitutes_ins.Code");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Grade_Grd.eduyearCode",
                table: "Grades",
                newName: "IX_Grades_Grd.eduyearCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Grade_Grd.insCode",
                table: "Grades",
                newName: "IX_Grades_Grd.insCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.ExamScore_Exsc.Studentid",
                table: "ExamScores",
                newName: "IX_ExamScores_Exsc.Studentid");

            migrationBuilder.RenameIndex(
                name: "IX_sm.ExamScore_Exsc.Examid",
                table: "ExamScores",
                newName: "IX_ExamScores_Exsc.Examid");

            migrationBuilder.RenameIndex(
                name: "IX_sm.ExamScore_DescriptiveScoreId",
                table: "ExamScores",
                newName: "IX_ExamScores_DescriptiveScoreId");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Exams_Ex.Year",
                table: "Exams",
                newName: "IX_Exams_Ex.Year");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Exams_WorkbookId",
                table: "Exams",
                newName: "IX_Exams_WorkbookId");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Exams_Ex.Teacher",
                table: "Exams",
                newName: "IX_Exams_Ex.Teacher");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Exams_ParentId",
                table: "Exams",
                newName: "IX_Exams_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Exams_Ex.Grade",
                table: "Exams",
                newName: "IX_Exams_Ex.Grade");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Exams_Ex.ExamTyp",
                table: "Exams",
                newName: "IX_Exams_Ex.ExamTyp");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Exams_Ex.Course",
                table: "Exams",
                newName: "IX_Exams_Ex.Course");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Exams_Ex.Class",
                table: "Exams",
                newName: "IX_Exams_Ex.Class");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Course_Crs.TeacherCode",
                table: "Courses",
                newName: "IX_Courses_Crs.TeacherCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Course_Crs.GradeCode",
                table: "Courses",
                newName: "IX_Courses_Crs.GradeCode");

            migrationBuilder.RenameIndex(
                name: "IX_Ind.Schedule_PostId",
                table: "Schedules",
                newName: "IX_Schedules_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Ind.Picture_PictureGalleryId",
                table: "Pictures",
                newName: "IX_Pictures_PictureGalleryId");

            migrationBuilder.RenameIndex(
                name: "IX_Ind.MainSlideShow_PostId",
                table: "MainSlideShows",
                newName: "IX_MainSlideShows_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Ind.Comment_PostId",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Ind.Comment_ParentId",
                table: "Comments",
                newName: "IX_Comments_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Fin.StdPayment_StdPayment.Student",
                table: "StdPayments",
                newName: "IX_StdPayments_StdPayment.Student");

            migrationBuilder.RenameIndex(
                name: "IX_Fin.StdPayment_StdPayment.PaymentTyp",
                table: "StdPayments",
                newName: "IX_StdPayments_StdPayment.PaymentTyp");

            migrationBuilder.RenameIndex(
                name: "IX_Fin.StdPayment_ContractId",
                table: "StdPayments",
                newName: "IX_StdPayments_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_Fin.Contract_ContractTypeId",
                table: "Contracts",
                newName: "IX_Contracts_ContractTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yeareducations",
                table: "Yeareducations",
                column: "edu.YeareduCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSchedules",
                table: "TimeSchedules",
                column: "Tsch.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeandDays",
                table: "TimeandDays",
                column: "Td.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketConversations",
                table: "TicketConversations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "Tch.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentInfos",
                table: "StudentInfos",
                column: "Stdinfo.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Std.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StdClassMngs",
                table: "StdClassMngs",
                column: "StdClassMng.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Salaries",
                table: "Salaries",
                column: "sal.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Que.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionOptions",
                table: "QuestionOptions",
                column: "QueOp.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgPeople",
                table: "OrgPeople",
                column: "Orgprs.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgCharts",
                table: "OrgCharts",
                column: "Org.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Insurances",
                table: "Insurances",
                column: "ins.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InsTitutes",
                table: "InsTitutes",
                column: "ins.AutoNum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grades",
                table: "Grades",
                column: "Grd.Autonumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamTypes",
                table: "ExamTypes",
                column: "Extyp.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamScores",
                table: "ExamScores",
                column: "Exsc.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exams",
                table: "Exams",
                column: "Exm.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Educations",
                table: "Educations",
                column: "edu.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Crs.Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PictureGalleries",
                table: "PictureGalleries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainSlideShows",
                table: "MainSlideShows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BestStudents",
                table: "BestStudents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advertisings",
                table: "Advertisings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StdPayments",
                table: "StdPayments",
                column: "StdPayment.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTypes",
                table: "PaymentTypes",
                column: "PatmentType.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractTypes",
                table: "ContractTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 10, 27, 22, 41, 58, 212, DateTimeKind.Local).AddTicks(7164), new Guid("5ad5bf14-28a3-4d4d-b0b1-3e7cc5c49363") });

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Questions_QuestionId",
                table: "Attributes",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Que.id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Courses_CourseId",
                table: "Categories",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_ExamTypes_ExamTypeId",
                table: "Categories",
                column: "ExamTypeId",
                principalTable: "ExamTypes",
                principalColumn: "Extyp.id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Grades_GradeId",
                table: "Categories",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_Courses_CourseId",
                table: "ClassBooks",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_Grades_GradeId",
                table: "ClassBooks",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_InsTitutes_InsTituteId",
                table: "ClassBooks",
                column: "InsTituteId",
                principalTable: "InsTitutes",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_Students_StudentId",
                table: "ClassBooks",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_Teachers_TeacherId",
                table: "ClassBooks",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_Yeareducations_YeareducationId",
                table: "ClassBooks",
                column: "YeareducationId",
                principalTable: "Yeareducations",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Grades_Grd.Id",
                table: "Classes",
                column: "Grd.Id",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments",
                column: "ParentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractTypes_ContractTypeId",
                table: "Contracts",
                column: "ContractTypeId",
                principalTable: "ContractTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Grades_Crs.GradeCode",
                table: "Courses",
                column: "Crs.GradeCode",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_Crs.TeacherCode",
                table: "Courses",
                column: "Crs.TeacherCode",
                principalTable: "Teachers",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Classes_Ex.Class",
                table: "Exams",
                column: "Ex.Class",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Courses_Ex.Course",
                table: "Exams",
                column: "Ex.Course",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamTypes_Ex.ExamTyp",
                table: "Exams",
                column: "Ex.ExamTyp",
                principalTable: "ExamTypes",
                principalColumn: "Extyp.id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Grades_Ex.Grade",
                table: "Exams",
                column: "Ex.Grade",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Exams_ParentId",
                table: "Exams",
                column: "ParentId",
                principalTable: "Exams",
                principalColumn: "Exm.id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Teachers_Ex.Teacher",
                table: "Exams",
                column: "Ex.Teacher",
                principalTable: "Teachers",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Workbooks_WorkbookId",
                table: "Exams",
                column: "WorkbookId",
                principalTable: "Workbooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ExamScores_Exams_Exsc.Examid",
                table: "ExamScores",
                column: "Exsc.Examid",
                principalTable: "Exams",
                principalColumn: "Exm.id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamScores_Students_Exsc.Studentid",
                table: "ExamScores",
                column: "Exsc.Studentid",
                principalTable: "Students",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_InsTitutes_Grd.insCode",
                table: "Grades",
                column: "Grd.insCode",
                principalTable: "InsTitutes",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Yeareducations_Grd.eduyearCode",
                table: "Grades",
                column: "Grd.eduyearCode",
                principalTable: "Yeareducations",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InsTitutes_InsTitutes_ins.Code",
                table: "InsTitutes",
                column: "ins.Code",
                principalTable: "InsTitutes",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MainSlideShows_Posts_PostId",
                table: "MainSlideShows",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationAgents_Students_StudentId",
                table: "NotificationAgents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_Courses_CourseId",
                table: "OnlineClasses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_Grades_GradeId",
                table: "OnlineClasses",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgCharts_OrgCharts_Org.ParentCode",
                table: "OrgCharts",
                column: "Org.ParentCode",
                principalTable: "OrgCharts",
                principalColumn: "Org.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgPeople_Educations_Orgprs.edu",
                table: "OrgPeople",
                column: "Orgprs.edu",
                principalTable: "Educations",
                principalColumn: "edu.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgPeople_Insurances_Orgprs.insuranceTyp",
                table: "OrgPeople",
                column: "Orgprs.insuranceTyp",
                principalTable: "Insurances",
                principalColumn: "ins.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgPeople_OrgCharts_Orgprs.ChartCode",
                table: "OrgPeople",
                column: "Orgprs.ChartCode",
                principalTable: "OrgCharts",
                principalColumn: "Org.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgPeople_Salaries_Orgprs.Salary",
                table: "OrgPeople",
                column: "Orgprs.Salary",
                principalTable: "Salaries",
                principalColumn: "sal.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_PictureGalleries_PictureGalleryId",
                table: "Pictures",
                column: "PictureGalleryId",
                principalTable: "PictureGalleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QueOp.Questionid",
                table: "QuestionOptions",
                column: "QueOp.Questionid",
                principalTable: "Questions",
                principalColumn: "Que.id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Courses_Que.Courseid",
                table: "Questions",
                column: "Que.Courseid",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Grades_Que.Gradeid",
                table: "Questions",
                column: "Que.Gradeid",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Posts_PostId",
                table: "Schedules",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StdClassMngs_Classes_Std.Classid",
                table: "StdClassMngs",
                column: "Std.Classid",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StdClassMngs_Grades_Std.Gradeid",
                table: "StdClassMngs",
                column: "Std.Gradeid",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StdClassMngs_InsTitutes_Std.insid",
                table: "StdClassMngs",
                column: "Std.insid",
                principalTable: "InsTitutes",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StdClassMngs_Students_Std.Code",
                table: "StdClassMngs",
                column: "Std.Code",
                principalTable: "Students",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StdClassMngs_StudentTypes_StudentTypeId",
                table: "StdClassMngs",
                column: "StudentTypeId",
                principalTable: "StudentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StdClassMngs_Yeareducations_Std.YearId",
                table: "StdClassMngs",
                column: "Std.YearId",
                principalTable: "Yeareducations",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StdPayments_Contracts_ContractId",
                table: "StdPayments",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StdPayments_PaymentTypes_StdPayment.PaymentTyp",
                table: "StdPayments",
                column: "StdPayment.PaymentTyp",
                principalTable: "PaymentTypes",
                principalColumn: "PatmentType.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StdPayments_Students_StdPayment.Student",
                table: "StdPayments",
                column: "StdPayment.Student",
                principalTable: "Students",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDailySchedules_Courses_CourseId",
                table: "StudentDailySchedules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDailySchedules_StdClassMngs_StdClassMngId",
                table: "StudentDailySchedules",
                column: "StdClassMngId",
                principalTable: "StdClassMngs",
                principalColumn: "StdClassMng.Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInfos_Students_Stdinfo.Code",
                table: "StudentInfos",
                column: "Stdinfo.Code",
                principalTable: "Students",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScores_StdClassMngs_StdClassMngId",
                table: "StudentScores",
                column: "StdClassMngId",
                principalTable: "StdClassMngs",
                principalColumn: "StdClassMng.Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScores_Teachers_TeacherId",
                table: "StudentScores",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_OrgPeople_Tch.PrsCode",
                table: "Teachers",
                column: "Tch.PrsCode",
                principalTable: "OrgPeople",
                principalColumn: "Orgprs.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketConversations_Tickets_TicketId",
                table: "TicketConversations",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSchedules_Courses_Tsch.CourseCode",
                table: "TimeSchedules",
                column: "Tsch.CourseCode",
                principalTable: "Courses",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSchedules_Teachers_Tsch.TeacherCode",
                table: "TimeSchedules",
                column: "Tsch.TeacherCode",
                principalTable: "Teachers",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSchedules_Teachers_TeacherId1",
                table: "TimeSchedules",
                column: "TeacherId1",
                principalTable: "Teachers",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSchedules_TimeandDays_Tsch.DaysId",
                table: "TimeSchedules",
                column: "Tsch.DaysId",
                principalTable: "TimeandDays",
                principalColumn: "Td.Autonum",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Questions_QuestionId",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Courses_CourseId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_ExamTypes_ExamTypeId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Grades_GradeId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_Courses_CourseId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_Grades_GradeId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_InsTitutes_InsTituteId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_Students_StudentId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_Teachers_TeacherId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_Yeareducations_YeareducationId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Grades_Grd.Id",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractTypes_ContractTypeId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Grades_Crs.GradeCode",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_Crs.TeacherCode",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Classes_Ex.Class",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Courses_Ex.Course",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamTypes_Ex.ExamTyp",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Grades_Ex.Grade",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Exams_ParentId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Teachers_Ex.Teacher",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Workbooks_WorkbookId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Yeareducations_Ex.Year",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamScores_DescriptiveScores_DescriptiveScoreId",
                table: "ExamScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamScores_Exams_Exsc.Examid",
                table: "ExamScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamScores_Students_Exsc.Studentid",
                table: "ExamScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_InsTitutes_Grd.insCode",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Yeareducations_Grd.eduyearCode",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_InsTitutes_InsTitutes_ins.Code",
                table: "InsTitutes");

            migrationBuilder.DropForeignKey(
                name: "FK_MainSlideShows_Posts_PostId",
                table: "MainSlideShows");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationAgents_Students_StudentId",
                table: "NotificationAgents");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_Courses_CourseId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_Grades_GradeId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgCharts_OrgCharts_Org.ParentCode",
                table: "OrgCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgPeople_Educations_Orgprs.edu",
                table: "OrgPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgPeople_Insurances_Orgprs.insuranceTyp",
                table: "OrgPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgPeople_OrgCharts_Orgprs.ChartCode",
                table: "OrgPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgPeople_Salaries_Orgprs.Salary",
                table: "OrgPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_PictureGalleries_PictureGalleryId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QueOp.Questionid",
                table: "QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Courses_Que.Courseid",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Grades_Que.Gradeid",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Posts_PostId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StdClassMngs_Classes_Std.Classid",
                table: "StdClassMngs");

            migrationBuilder.DropForeignKey(
                name: "FK_StdClassMngs_Grades_Std.Gradeid",
                table: "StdClassMngs");

            migrationBuilder.DropForeignKey(
                name: "FK_StdClassMngs_InsTitutes_Std.insid",
                table: "StdClassMngs");

            migrationBuilder.DropForeignKey(
                name: "FK_StdClassMngs_Students_Std.Code",
                table: "StdClassMngs");

            migrationBuilder.DropForeignKey(
                name: "FK_StdClassMngs_StudentTypes_StudentTypeId",
                table: "StdClassMngs");

            migrationBuilder.DropForeignKey(
                name: "FK_StdClassMngs_Yeareducations_Std.YearId",
                table: "StdClassMngs");

            migrationBuilder.DropForeignKey(
                name: "FK_StdPayments_Contracts_ContractId",
                table: "StdPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StdPayments_PaymentTypes_StdPayment.PaymentTyp",
                table: "StdPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StdPayments_Students_StdPayment.Student",
                table: "StdPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDailySchedules_Courses_CourseId",
                table: "StudentDailySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDailySchedules_StdClassMngs_StdClassMngId",
                table: "StudentDailySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentInfos_Students_Stdinfo.Code",
                table: "StudentInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentScores_StdClassMngs_StdClassMngId",
                table: "StudentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentScores_Teachers_TeacherId",
                table: "StudentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_OrgPeople_Tch.PrsCode",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketConversations_Tickets_TicketId",
                table: "TicketConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSchedules_Courses_Tsch.CourseCode",
                table: "TimeSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSchedules_Teachers_Tsch.TeacherCode",
                table: "TimeSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSchedules_Teachers_TeacherId1",
                table: "TimeSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSchedules_TimeandDays_Tsch.DaysId",
                table: "TimeSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yeareducations",
                table: "Yeareducations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSchedules",
                table: "TimeSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeandDays",
                table: "TimeandDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketConversations",
                table: "TicketConversations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentInfos",
                table: "StudentInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StdPayments",
                table: "StdPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StdClassMngs",
                table: "StdClassMngs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Salaries",
                table: "Salaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionOptions",
                table: "QuestionOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PictureGalleries",
                table: "PictureGalleries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTypes",
                table: "PaymentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgPeople",
                table: "OrgPeople");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgCharts",
                table: "OrgCharts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MainSlideShows",
                table: "MainSlideShows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Insurances",
                table: "Insurances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InsTitutes",
                table: "InsTitutes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grades",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamTypes",
                table: "ExamTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamScores",
                table: "ExamScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exams",
                table: "Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Educations",
                table: "Educations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractTypes",
                table: "ContractTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BestStudents",
                table: "BestStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advertisings",
                table: "Advertisings");

            migrationBuilder.RenameTable(
                name: "Yeareducations",
                newName: "sm.Yeareducation");

            migrationBuilder.RenameTable(
                name: "TimeSchedules",
                newName: "sm.TimeSchedule");

            migrationBuilder.RenameTable(
                name: "TimeandDays",
                newName: "sm.TimeandDays");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "sm.Ticket");

            migrationBuilder.RenameTable(
                name: "TicketConversations",
                newName: "sm.TicketConversation");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "sm.Teacher");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "sm.Student");

            migrationBuilder.RenameTable(
                name: "StudentInfos",
                newName: "sm.StudentInfo");

            migrationBuilder.RenameTable(
                name: "StdPayments",
                newName: "Fin.StdPayment");

            migrationBuilder.RenameTable(
                name: "StdClassMngs",
                newName: "sm.StdClassMng");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Ind.Schedule");

            migrationBuilder.RenameTable(
                name: "Salaries",
                newName: "sm.Salary");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "sm.Questions");

            migrationBuilder.RenameTable(
                name: "QuestionOptions",
                newName: "sm.QuestionOptions");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Ind.Post");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "Ind.Picture");

            migrationBuilder.RenameTable(
                name: "PictureGalleries",
                newName: "Ind.PictureGallery");

            migrationBuilder.RenameTable(
                name: "PaymentTypes",
                newName: "Fin.PaymentType");

            migrationBuilder.RenameTable(
                name: "OrgPeople",
                newName: "sm.OrgPerson");

            migrationBuilder.RenameTable(
                name: "OrgCharts",
                newName: "sm.OrgChart");

            migrationBuilder.RenameTable(
                name: "MainSlideShows",
                newName: "Ind.MainSlideShow");

            migrationBuilder.RenameTable(
                name: "Insurances",
                newName: "sm.InsuranceTable");

            migrationBuilder.RenameTable(
                name: "InsTitutes",
                newName: "sm.insTitute");

            migrationBuilder.RenameTable(
                name: "Grades",
                newName: "sm.Grade");

            migrationBuilder.RenameTable(
                name: "ExamTypes",
                newName: "sm.ExamTyp");

            migrationBuilder.RenameTable(
                name: "ExamScores",
                newName: "sm.ExamScore");

            migrationBuilder.RenameTable(
                name: "Exams",
                newName: "sm.Exams");

            migrationBuilder.RenameTable(
                name: "Educations",
                newName: "sm.EducationTable");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "sm.Course");

            migrationBuilder.RenameTable(
                name: "ContractTypes",
                newName: "Fin.ContractType");

            migrationBuilder.RenameTable(
                name: "Contracts",
                newName: "Fin.Contract");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Ind.Comment");

            migrationBuilder.RenameTable(
                name: "BestStudents",
                newName: "Ind.BestStudent");

            migrationBuilder.RenameTable(
                name: "Advertisings",
                newName: "Ind.Advertising");

            migrationBuilder.RenameIndex(
                name: "IX_TimeSchedules_Tsch.DaysId",
                table: "sm.TimeSchedule",
                newName: "IX_sm.TimeSchedule_Tsch.DaysId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeSchedules_TeacherId1",
                table: "sm.TimeSchedule",
                newName: "IX_sm.TimeSchedule_TeacherId1");

            migrationBuilder.RenameIndex(
                name: "IX_TimeSchedules_Tsch.TeacherCode",
                table: "sm.TimeSchedule",
                newName: "IX_sm.TimeSchedule_Tsch.TeacherCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimeSchedules_Tsch.CourseCode",
                table: "sm.TimeSchedule",
                newName: "IX_sm.TimeSchedule_Tsch.CourseCode");

            migrationBuilder.RenameIndex(
                name: "IX_TicketConversations_TicketId",
                table: "sm.TicketConversation",
                newName: "IX_sm.TicketConversation_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Teachers_Tch.PrsCode",
                table: "sm.Teacher",
                newName: "IX_sm.Teacher_Tch.PrsCode");

            migrationBuilder.RenameIndex(
                name: "IX_StudentInfos_Stdinfo.Code",
                table: "sm.StudentInfo",
                newName: "IX_sm.StudentInfo_Stdinfo.Code");

            migrationBuilder.RenameIndex(
                name: "IX_StdPayments_StdPayment.Student",
                table: "Fin.StdPayment",
                newName: "IX_Fin.StdPayment_StdPayment.Student");

            migrationBuilder.RenameIndex(
                name: "IX_StdPayments_StdPayment.PaymentTyp",
                table: "Fin.StdPayment",
                newName: "IX_Fin.StdPayment_StdPayment.PaymentTyp");

            migrationBuilder.RenameIndex(
                name: "IX_StdPayments_ContractId",
                table: "Fin.StdPayment",
                newName: "IX_Fin.StdPayment_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_StdClassMngs_Std.YearId",
                table: "sm.StdClassMng",
                newName: "IX_sm.StdClassMng_Std.YearId");

            migrationBuilder.RenameIndex(
                name: "IX_StdClassMngs_StudentTypeId",
                table: "sm.StdClassMng",
                newName: "IX_sm.StdClassMng_StudentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_StdClassMngs_Std.Code",
                table: "sm.StdClassMng",
                newName: "IX_sm.StdClassMng_Std.Code");

            migrationBuilder.RenameIndex(
                name: "IX_StdClassMngs_Std.insid",
                table: "sm.StdClassMng",
                newName: "IX_sm.StdClassMng_Std.insid");

            migrationBuilder.RenameIndex(
                name: "IX_StdClassMngs_Std.Gradeid",
                table: "sm.StdClassMng",
                newName: "IX_sm.StdClassMng_Std.Gradeid");

            migrationBuilder.RenameIndex(
                name: "IX_StdClassMngs_Std.Classid",
                table: "sm.StdClassMng",
                newName: "IX_sm.StdClassMng_Std.Classid");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_PostId",
                table: "Ind.Schedule",
                newName: "IX_Ind.Schedule_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_Que.Gradeid",
                table: "sm.Questions",
                newName: "IX_sm.Questions_Que.Gradeid");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_Que.Courseid",
                table: "sm.Questions",
                newName: "IX_sm.Questions_Que.Courseid");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionOptions_QueOp.Questionid",
                table: "sm.QuestionOptions",
                newName: "IX_sm.QuestionOptions_QueOp.Questionid");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_PictureGalleryId",
                table: "Ind.Picture",
                newName: "IX_Ind.Picture_PictureGalleryId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgPeople_Orgprs.Salary",
                table: "sm.OrgPerson",
                newName: "IX_sm.OrgPerson_Orgprs.Salary");

            migrationBuilder.RenameIndex(
                name: "IX_OrgPeople_Orgprs.ChartCode",
                table: "sm.OrgPerson",
                newName: "IX_sm.OrgPerson_Orgprs.ChartCode");

            migrationBuilder.RenameIndex(
                name: "IX_OrgPeople_Orgprs.insuranceTyp",
                table: "sm.OrgPerson",
                newName: "IX_sm.OrgPerson_Orgprs.insuranceTyp");

            migrationBuilder.RenameIndex(
                name: "IX_OrgPeople_Orgprs.edu",
                table: "sm.OrgPerson",
                newName: "IX_sm.OrgPerson_Orgprs.edu");

            migrationBuilder.RenameIndex(
                name: "IX_OrgCharts_Org.ParentCode",
                table: "sm.OrgChart",
                newName: "IX_sm.OrgChart_Org.ParentCode");

            migrationBuilder.RenameIndex(
                name: "IX_MainSlideShows_PostId",
                table: "Ind.MainSlideShow",
                newName: "IX_Ind.MainSlideShow_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_InsTitutes_ins.Code",
                table: "sm.insTitute",
                newName: "IX_sm.insTitute_ins.Code");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_Grd.eduyearCode",
                table: "sm.Grade",
                newName: "IX_sm.Grade_Grd.eduyearCode");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_Grd.insCode",
                table: "sm.Grade",
                newName: "IX_sm.Grade_Grd.insCode");

            migrationBuilder.RenameIndex(
                name: "IX_ExamScores_Exsc.Studentid",
                table: "sm.ExamScore",
                newName: "IX_sm.ExamScore_Exsc.Studentid");

            migrationBuilder.RenameIndex(
                name: "IX_ExamScores_Exsc.Examid",
                table: "sm.ExamScore",
                newName: "IX_sm.ExamScore_Exsc.Examid");

            migrationBuilder.RenameIndex(
                name: "IX_ExamScores_DescriptiveScoreId",
                table: "sm.ExamScore",
                newName: "IX_sm.ExamScore_DescriptiveScoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_Ex.Year",
                table: "sm.Exams",
                newName: "IX_sm.Exams_Ex.Year");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_WorkbookId",
                table: "sm.Exams",
                newName: "IX_sm.Exams_WorkbookId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_Ex.Teacher",
                table: "sm.Exams",
                newName: "IX_sm.Exams_Ex.Teacher");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_ParentId",
                table: "sm.Exams",
                newName: "IX_sm.Exams_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_Ex.Grade",
                table: "sm.Exams",
                newName: "IX_sm.Exams_Ex.Grade");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_Ex.ExamTyp",
                table: "sm.Exams",
                newName: "IX_sm.Exams_Ex.ExamTyp");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_Ex.Course",
                table: "sm.Exams",
                newName: "IX_sm.Exams_Ex.Course");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_Ex.Class",
                table: "sm.Exams",
                newName: "IX_sm.Exams_Ex.Class");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_Crs.TeacherCode",
                table: "sm.Course",
                newName: "IX_sm.Course_Crs.TeacherCode");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_Crs.GradeCode",
                table: "sm.Course",
                newName: "IX_sm.Course_Crs.GradeCode");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_ContractTypeId",
                table: "Fin.Contract",
                newName: "IX_Fin.Contract_ContractTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "Ind.Comment",
                newName: "IX_Ind.Comment_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ParentId",
                table: "Ind.Comment",
                newName: "IX_Ind.Comment_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Yeareducation",
                table: "sm.Yeareducation",
                column: "edu.YeareduCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.TimeSchedule",
                table: "sm.TimeSchedule",
                column: "Tsch.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.TimeandDays",
                table: "sm.TimeandDays",
                column: "Td.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Ticket",
                table: "sm.Ticket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.TicketConversation",
                table: "sm.TicketConversation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Teacher",
                table: "sm.Teacher",
                column: "Tch.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Student",
                table: "sm.Student",
                column: "Std.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.StudentInfo",
                table: "sm.StudentInfo",
                column: "Stdinfo.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fin.StdPayment",
                table: "Fin.StdPayment",
                column: "StdPayment.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.StdClassMng",
                table: "sm.StdClassMng",
                column: "StdClassMng.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ind.Schedule",
                table: "Ind.Schedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Salary",
                table: "sm.Salary",
                column: "sal.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Questions",
                table: "sm.Questions",
                column: "Que.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.QuestionOptions",
                table: "sm.QuestionOptions",
                column: "QueOp.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ind.Post",
                table: "Ind.Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ind.Picture",
                table: "Ind.Picture",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ind.PictureGallery",
                table: "Ind.PictureGallery",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fin.PaymentType",
                table: "Fin.PaymentType",
                column: "PatmentType.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.OrgPerson",
                table: "sm.OrgPerson",
                column: "Orgprs.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.OrgChart",
                table: "sm.OrgChart",
                column: "Org.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ind.MainSlideShow",
                table: "Ind.MainSlideShow",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.InsuranceTable",
                table: "sm.InsuranceTable",
                column: "ins.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.insTitute",
                table: "sm.insTitute",
                column: "ins.AutoNum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Grade",
                table: "sm.Grade",
                column: "Grd.Autonumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.ExamTyp",
                table: "sm.ExamTyp",
                column: "Extyp.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.ExamScore",
                table: "sm.ExamScore",
                column: "Exsc.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Exams",
                table: "sm.Exams",
                column: "Exm.id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.EducationTable",
                table: "sm.EducationTable",
                column: "edu.Autonum");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Course",
                table: "sm.Course",
                column: "Crs.Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fin.ContractType",
                table: "Fin.ContractType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fin.Contract",
                table: "Fin.Contract",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ind.Comment",
                table: "Ind.Comment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ind.BestStudent",
                table: "Ind.BestStudent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ind.Advertising",
                table: "Ind.Advertising",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 10, 27, 22, 28, 43, 378, DateTimeKind.Local).AddTicks(4848), new Guid("44bd5e2a-e419-4312-abc8-5cdbd6ab6966") });

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_sm.Questions_QuestionId",
                table: "Attributes",
                column: "QuestionId",
                principalTable: "sm.Questions",
                principalColumn: "Que.id",
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
                name: "FK_Categories_sm.Grade_GradeId",
                table: "Categories",
                column: "GradeId",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_sm.Course_CourseId",
                table: "ClassBooks",
                column: "CourseId",
                principalTable: "sm.Course",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_sm.Grade_GradeId",
                table: "ClassBooks",
                column: "GradeId",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_sm.insTitute_InsTituteId",
                table: "ClassBooks",
                column: "InsTituteId",
                principalTable: "sm.insTitute",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_sm.Student_StudentId",
                table: "ClassBooks",
                column: "StudentId",
                principalTable: "sm.Student",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_sm.Teacher_TeacherId",
                table: "ClassBooks",
                column: "TeacherId",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_sm.Yeareducation_YeareducationId",
                table: "ClassBooks",
                column: "YeareducationId",
                principalTable: "sm.Yeareducation",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_sm.Grade_Grd.Id",
                table: "Classes",
                column: "Grd.Id",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fin.Contract_Fin.ContractType_ContractTypeId",
                table: "Fin.Contract",
                column: "ContractTypeId",
                principalTable: "Fin.ContractType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fin.StdPayment_Fin.Contract_ContractId",
                table: "Fin.StdPayment",
                column: "ContractId",
                principalTable: "Fin.Contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fin.StdPayment_Fin.PaymentType_StdPayment.PaymentTyp",
                table: "Fin.StdPayment",
                column: "StdPayment.PaymentTyp",
                principalTable: "Fin.PaymentType",
                principalColumn: "PatmentType.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fin.StdPayment_sm.Student_StdPayment.Student",
                table: "Fin.StdPayment",
                column: "StdPayment.Student",
                principalTable: "sm.Student",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.Comment_Ind.Comment_ParentId",
                table: "Ind.Comment",
                column: "ParentId",
                principalTable: "Ind.Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.Comment_Ind.Post_PostId",
                table: "Ind.Comment",
                column: "PostId",
                principalTable: "Ind.Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.MainSlideShow_Ind.Post_PostId",
                table: "Ind.MainSlideShow",
                column: "PostId",
                principalTable: "Ind.Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.Picture_Ind.PictureGallery_PictureGalleryId",
                table: "Ind.Picture",
                column: "PictureGalleryId",
                principalTable: "Ind.PictureGallery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.Schedule_Ind.Post_PostId",
                table: "Ind.Schedule",
                column: "PostId",
                principalTable: "Ind.Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationAgents_sm.Student_StudentId",
                table: "NotificationAgents",
                column: "StudentId",
                principalTable: "sm.Student",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_sm.Course_CourseId",
                table: "OnlineClasses",
                column: "CourseId",
                principalTable: "sm.Course",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_sm.Grade_GradeId",
                table: "OnlineClasses",
                column: "GradeId",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Course_sm.Grade_Crs.GradeCode",
                table: "sm.Course",
                column: "Crs.GradeCode",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Course_sm.Teacher_Crs.TeacherCode",
                table: "sm.Course",
                column: "Crs.TeacherCode",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_Classes_Ex.Class",
                table: "sm.Exams",
                column: "Ex.Class",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_sm.Course_Ex.Course",
                table: "sm.Exams",
                column: "Ex.Course",
                principalTable: "sm.Course",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_sm.ExamTyp_Ex.ExamTyp",
                table: "sm.Exams",
                column: "Ex.ExamTyp",
                principalTable: "sm.ExamTyp",
                principalColumn: "Extyp.id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_sm.Grade_Ex.Grade",
                table: "sm.Exams",
                column: "Ex.Grade",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_sm.Exams_ParentId",
                table: "sm.Exams",
                column: "ParentId",
                principalTable: "sm.Exams",
                principalColumn: "Exm.id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_sm.Teacher_Ex.Teacher",
                table: "sm.Exams",
                column: "Ex.Teacher",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_Workbooks_WorkbookId",
                table: "sm.Exams",
                column: "WorkbookId",
                principalTable: "Workbooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_sm.Yeareducation_Ex.Year",
                table: "sm.Exams",
                column: "Ex.Year",
                principalTable: "sm.Yeareducation",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.ExamScore_DescriptiveScores_DescriptiveScoreId",
                table: "sm.ExamScore",
                column: "DescriptiveScoreId",
                principalTable: "DescriptiveScores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.ExamScore_sm.Exams_Exsc.Examid",
                table: "sm.ExamScore",
                column: "Exsc.Examid",
                principalTable: "sm.Exams",
                principalColumn: "Exm.id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.ExamScore_sm.Student_Exsc.Studentid",
                table: "sm.ExamScore",
                column: "Exsc.Studentid",
                principalTable: "sm.Student",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Grade_sm.insTitute_Grd.insCode",
                table: "sm.Grade",
                column: "Grd.insCode",
                principalTable: "sm.insTitute",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Grade_sm.Yeareducation_Grd.eduyearCode",
                table: "sm.Grade",
                column: "Grd.eduyearCode",
                principalTable: "sm.Yeareducation",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.insTitute_sm.insTitute_ins.Code",
                table: "sm.insTitute",
                column: "ins.Code",
                principalTable: "sm.insTitute",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.OrgChart_sm.OrgChart_Org.ParentCode",
                table: "sm.OrgChart",
                column: "Org.ParentCode",
                principalTable: "sm.OrgChart",
                principalColumn: "Org.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.OrgPerson_sm.EducationTable_Orgprs.edu",
                table: "sm.OrgPerson",
                column: "Orgprs.edu",
                principalTable: "sm.EducationTable",
                principalColumn: "edu.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.OrgPerson_sm.InsuranceTable_Orgprs.insuranceTyp",
                table: "sm.OrgPerson",
                column: "Orgprs.insuranceTyp",
                principalTable: "sm.InsuranceTable",
                principalColumn: "ins.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.OrgPerson_sm.OrgChart_Orgprs.ChartCode",
                table: "sm.OrgPerson",
                column: "Orgprs.ChartCode",
                principalTable: "sm.OrgChart",
                principalColumn: "Org.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.OrgPerson_sm.Salary_Orgprs.Salary",
                table: "sm.OrgPerson",
                column: "Orgprs.Salary",
                principalTable: "sm.Salary",
                principalColumn: "sal.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.QuestionOptions_sm.Questions_QueOp.Questionid",
                table: "sm.QuestionOptions",
                column: "QueOp.Questionid",
                principalTable: "sm.Questions",
                principalColumn: "Que.id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Questions_sm.Course_Que.Courseid",
                table: "sm.Questions",
                column: "Que.Courseid",
                principalTable: "sm.Course",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Questions_sm.Grade_Que.Gradeid",
                table: "sm.Questions",
                column: "Que.Gradeid",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_Classes_Std.Classid",
                table: "sm.StdClassMng",
                column: "Std.Classid",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_sm.Grade_Std.Gradeid",
                table: "sm.StdClassMng",
                column: "Std.Gradeid",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_sm.insTitute_Std.insid",
                table: "sm.StdClassMng",
                column: "Std.insid",
                principalTable: "sm.insTitute",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_sm.Student_Std.Code",
                table: "sm.StdClassMng",
                column: "Std.Code",
                principalTable: "sm.Student",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_StudentTypes_StudentTypeId",
                table: "sm.StdClassMng",
                column: "StudentTypeId",
                principalTable: "StudentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_sm.Yeareducation_Std.YearId",
                table: "sm.StdClassMng",
                column: "Std.YearId",
                principalTable: "sm.Yeareducation",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StudentInfo_sm.Student_Stdinfo.Code",
                table: "sm.StudentInfo",
                column: "Stdinfo.Code",
                principalTable: "sm.Student",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Teacher_sm.OrgPerson_Tch.PrsCode",
                table: "sm.Teacher",
                column: "Tch.PrsCode",
                principalTable: "sm.OrgPerson",
                principalColumn: "Orgprs.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.TicketConversation_sm.Ticket_TicketId",
                table: "sm.TicketConversation",
                column: "TicketId",
                principalTable: "sm.Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.TimeSchedule_sm.Course_Tsch.CourseCode",
                table: "sm.TimeSchedule",
                column: "Tsch.CourseCode",
                principalTable: "sm.Course",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_Tsch.TeacherCode",
                table: "sm.TimeSchedule",
                column: "Tsch.TeacherCode",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.TimeSchedule_sm.Teacher_TeacherId1",
                table: "sm.TimeSchedule",
                column: "TeacherId1",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.TimeSchedule_sm.TimeandDays_Tsch.DaysId",
                table: "sm.TimeSchedule",
                column: "Tsch.DaysId",
                principalTable: "sm.TimeandDays",
                principalColumn: "Td.Autonum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDailySchedules_sm.Course_CourseId",
                table: "StudentDailySchedules",
                column: "CourseId",
                principalTable: "sm.Course",
                principalColumn: "Crs.Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDailySchedules_sm.StdClassMng_StdClassMngId",
                table: "StudentDailySchedules",
                column: "StdClassMngId",
                principalTable: "sm.StdClassMng",
                principalColumn: "StdClassMng.Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScores_sm.StdClassMng_StdClassMngId",
                table: "StudentScores",
                column: "StdClassMngId",
                principalTable: "sm.StdClassMng",
                principalColumn: "StdClassMng.Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScores_sm.Teacher_TeacherId",
                table: "StudentScores",
                column: "TeacherId",
                principalTable: "sm.Teacher",
                principalColumn: "Tch.Autonum",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
