using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate28_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ParentAccess",
                table: "sm.Student",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "StudentAccess",
                table: "sm.Student",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 27, 11, 38, 26, 307, DateTimeKind.Local).AddTicks(6718), new Guid("e0e60b55-e0ef-426b-bd65-2903f7443b09") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentAccess",
                table: "sm.Student");

            migrationBuilder.DropColumn(
                name: "StudentAccess",
                table: "sm.Student");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 26, 11, 35, 4, 30, DateTimeKind.Local).AddTicks(5429), new Guid("40566422-5366-4254-8e50-e75a04478b8b") });
        }
    }
}
