using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate47_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowAvgOfExam",
                table: "Exams",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBackStepAllowed",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 31, 20, 19, 57, 293, DateTimeKind.Local).AddTicks(2450), new Guid("385adccd-5c71-456e-8ada-5acbe24c3ef8") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowAvgOfExam",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "IsBackStepAllowed",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 15, 8, 20, 53, 241, DateTimeKind.Local).AddTicks(7904), new Guid("026af37b-c02c-431a-b4aa-19a98788e77f") });
        }
    }
}
