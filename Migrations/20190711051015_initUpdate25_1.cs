using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate25_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "haveVideo",
                table: "Ind.Post",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 11, 9, 40, 13, 904, DateTimeKind.Local).AddTicks(4624), new Guid("1b8a7fd2-889d-4b83-bc18-d083d9f64012") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "haveVideo",
                table: "Ind.Post");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 10, 22, 28, 26, 671, DateTimeKind.Local).AddTicks(2054), new Guid("255e2ab2-5945-4a8c-9bd2-5be0224056bc") });
        }
    }
}
