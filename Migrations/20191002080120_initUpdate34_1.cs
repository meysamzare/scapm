using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate34_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "View_StudentFinancialWidget",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 2, 11, 31, 19, 54, DateTimeKind.Local).AddTicks(110), new Guid("dafe2757-8729-4d09-b1f2-a770c241d20c") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "View_StudentFinancialWidget",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 26, 16, 14, 28, 431, DateTimeKind.Local).AddTicks(9230), new Guid("06923c76-53fe-47f2-bc41-330db57ab5ed") });
        }
    }
}
