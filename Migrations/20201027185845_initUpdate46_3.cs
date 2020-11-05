using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate46_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_sm.Class_ClassId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_sm.Class_ClassId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_sm.Class_ClassId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Class_sm.Grade_Grd.Id",
                table: "sm.Class");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_sm.Class_Ex.Class",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_sm.Class_Std.Classid",
                table: "sm.StdClassMng");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sm.Class",
                table: "sm.Class");

            // migrationBuilder.RenameTable(
            //     name: "sm.Salary",
            //     schema: "tahaschi_meysam",
            //     newName: "sm.Salary");

            migrationBuilder.RenameTable(
                name: "sm.Class",
                newName: "Classes");

            migrationBuilder.RenameIndex(
                name: "IX_sm.Class_Grd.Id",
                table: "Classes",
                newName: "IX_Classes_Grd.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classes",
                table: "Classes",
                column: "Cls.Autonumber");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 10, 27, 22, 28, 43, 378, DateTimeKind.Local).AddTicks(4848), new Guid("44bd5e2a-e419-4312-abc8-5cdbd6ab6966") });

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Classes_ClassId",
                table: "Categories",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_Classes_ClassId",
                table: "ClassBooks",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_sm.Grade_Grd.Id",
                table: "Classes",
                column: "Grd.Id",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_Classes_ClassId",
                table: "OnlineClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_Classes_Ex.Class",
                table: "sm.Exams",
                column: "Ex.Class",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_Classes_Std.Classid",
                table: "sm.StdClassMng",
                column: "Std.Classid",
                principalTable: "Classes",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Classes_ClassId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassBooks_Classes_ClassId",
                table: "ClassBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_sm.Grade_Grd.Id",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_Classes_ClassId",
                table: "OnlineClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.Exams_Classes_Ex.Class",
                table: "sm.Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_Classes_Std.Classid",
                table: "sm.StdClassMng");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classes",
                table: "Classes");

            migrationBuilder.EnsureSchema(
                name: "tahaschi_meysam");

            // migrationBuilder.RenameTable(
            //     name: "sm.Salary",
            //     newName: "sm.Salary",
            //     newSchema: "tahaschi_meysam");

            migrationBuilder.RenameTable(
                name: "Classes",
                newName: "sm.Class");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_Grd.Id",
                table: "sm.Class",
                newName: "IX_sm.Class_Grd.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sm.Class",
                table: "sm.Class",
                column: "Cls.Autonumber");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 10, 22, 18, 33, 49, 320, DateTimeKind.Local).AddTicks(642), new Guid("e3980253-beae-44d6-a381-4f94ff642d4e") });

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_sm.Class_ClassId",
                table: "Categories",
                column: "ClassId",
                principalTable: "sm.Class",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassBooks_sm.Class_ClassId",
                table: "ClassBooks",
                column: "ClassId",
                principalTable: "sm.Class",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_sm.Class_ClassId",
                table: "OnlineClasses",
                column: "ClassId",
                principalTable: "sm.Class",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Class_sm.Grade_Grd.Id",
                table: "sm.Class",
                column: "Grd.Id",
                principalTable: "sm.Grade",
                principalColumn: "Grd.Autonumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Exams_sm.Class_Ex.Class",
                table: "sm.Exams",
                column: "Ex.Class",
                principalTable: "sm.Class",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_sm.Class_Std.Classid",
                table: "sm.StdClassMng",
                column: "Std.Classid",
                principalTable: "sm.Class",
                principalColumn: "Cls.Autonumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
