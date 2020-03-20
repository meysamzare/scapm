using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate33_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "sm.Yeareducation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 24, 23, 34, 19, 608, DateTimeKind.Local).AddTicks(8648), new Guid("55d46007-3464-4dcf-9c03-dc31a7ca60df") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "sm.Yeareducation");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 24, 18, 6, 30, 129, DateTimeKind.Local).AddTicks(2866), new Guid("262a9d7e-5ed6-4cb5-be48-88fdad13e336") });
        }
    }
}
