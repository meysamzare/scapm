using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate25_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostType",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId", "Username" },
                values: new object[] { new DateTime(2019, 7, 14, 23, 52, 3, 101, DateTimeKind.Local).AddTicks(2164), new Guid("a6ab64b6-5946-426d-a224-848130ce3291"), "meysam" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostType",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId", "Username" },
                values: new object[] { new DateTime(2019, 7, 11, 9, 40, 13, 904, DateTimeKind.Local).AddTicks(4624), new Guid("1b8a7fd2-889d-4b83-bc18-d083d9f64012"), "meysam1" });
        }
    }
}
