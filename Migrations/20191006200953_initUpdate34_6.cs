using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate34_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 6, 23, 39, 52, 124, DateTimeKind.Local).AddTicks(4842), new Guid("9d962916-c4a6-4565-93ef-0a26c9415f95") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 6, 23, 20, 40, 353, DateTimeKind.Local).AddTicks(7673), new Guid("e8bba7e7-6e42-4d47-a1dc-d1cfc132a02d") });
        }
    }
}
