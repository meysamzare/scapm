using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate27_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_StudentType_StudentTypeId",
                table: "sm.StdClassMng");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentType",
                table: "StudentType");

            migrationBuilder.RenameTable(
                name: "StudentType",
                newName: "StudentTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentTypes",
                table: "StudentTypes",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 5, 18, 4, 9, 335, DateTimeKind.Local).AddTicks(1150), new Guid("794b2a3e-8c81-4f58-924b-c9cec5c0d101") });

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_StudentTypes_StudentTypeId",
                table: "sm.StdClassMng",
                column: "StudentTypeId",
                principalTable: "StudentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_StudentTypes_StudentTypeId",
                table: "sm.StdClassMng");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentTypes",
                table: "StudentTypes");

            migrationBuilder.RenameTable(
                name: "StudentTypes",
                newName: "StudentType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentType",
                table: "StudentType",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 5, 18, 2, 29, 474, DateTimeKind.Local).AddTicks(3796), new Guid("1571669b-ad18-438c-9bfe-eb9a6fe863b7") });

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_StudentType_StudentTypeId",
                table: "sm.StdClassMng",
                column: "StudentTypeId",
                principalTable: "StudentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
