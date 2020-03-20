using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate9_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Grades_Grd.Id",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_sm.insTitute_Grd.insCode",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_sm.Yeareducation_Grd.eduyearCode",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grades",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classes",
                table: "Classes");

            migrationBuilder.RenameTable(
                name: "Grades",
                newName: "sm.Grade");

            migrationBuilder.RenameTable(
                name: "Classes",
                newName: "sm.Class");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_Grd.eduyearCode",
                table: "sm.Grade",
                newName: "IX_sm.Grade_Grd.eduyearCode");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_Grd.insCode",
                table: "sm.Grade",
                newName: "IX_sm.Grade_Grd.insCode");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_Grd.Id",
                table: "sm.Class",
                newName: "IX_sm.Class_Grd.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Grade",
                table: "sm.Grade",
                column: "Grd.Autonumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Class",
                table: "sm.Class",
                column: "Cls.Autonumber");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 25, 21, 48, 8, 830, DateTimeKind.Local).AddTicks(5683), new Guid("8f5170c3-686b-46ec-a6ba-0a144bd9a797") });

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Class_sm.Grade_Grd.Id",
                table: "sm.Class",
                column: "Grd.Id",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.Class_sm.Grade_Grd.Id",
                table: "sm.Class");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Grade_sm.insTitute_Grd.insCode",
                table: "sm.Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Grade_sm.Yeareducation_Grd.eduyearCode",
                table: "sm.Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Grade",
                table: "sm.Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Class",
                table: "sm.Class");

            migrationBuilder.RenameTable(
                name: "sm.Grade",
                newName: "Grades");

            migrationBuilder.RenameTable(
                name: "sm.Class",
                newName: "Classes");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Grade_Grd.eduyearCode",
                table: "Grades",
                newName: "IX_Grades_Grd.eduyearCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Grade_Grd.insCode",
                table: "Grades",
                newName: "IX_Grades_Grd.insCode");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Class_Grd.Id",
                table: "Classes",
                newName: "IX_Classes_Grd.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grades",
                table: "Grades",
                column: "Grd.Autonumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classes",
                table: "Classes",
                column: "Cls.Autonumber");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 25, 21, 45, 18, 761, DateTimeKind.Local).AddTicks(2629), new Guid("c073f383-0c7b-47b5-8e37-0d9cac7dd826") });

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Grades_Grd.Id",
                table: "Classes",
                column: "Grd.Id",
                principalTable: "Grades",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_sm.insTitute_Grd.insCode",
                table: "Grades",
                column: "Grd.insCode",
                principalTable: "sm.insTitute",
                principalColumn: "ins.AutoNum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_sm.Yeareducation_Grd.eduyearCode",
                table: "Grades",
                column: "Grd.eduyearCode",
                principalTable: "sm.Yeareducation",
                principalColumn: "edu.YeareduCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
