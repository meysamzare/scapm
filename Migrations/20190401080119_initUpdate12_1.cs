using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate12_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Stdinfo.Desc",
                table: "sm.StudentInfo",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 4, 1, 12, 31, 19, 138, DateTimeKind.Local).AddTicks(5213), new Guid("1ba1bbc1-ecb5-4832-ae9f-79e06f537931") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stdinfo.Desc",
                table: "sm.StudentInfo");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 31, 22, 0, 29, 374, DateTimeKind.Local).AddTicks(6324), new Guid("8edd5c93-9f1c-4116-9112-1e1e806f611c") });
        }
    }
}
