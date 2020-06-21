using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "ItemAttributes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TeachersIdAccess",
                table: "Categories",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 6, 19, 11, 52, 34, 137, DateTimeKind.Local).AddTicks(8558), new Guid("223caf13-9849-4e94-8fb2-6d054b066786") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "ItemAttributes");

            migrationBuilder.DropColumn(
                name: "TeachersIdAccess",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 6, 16, 12, 1, 15, 862, DateTimeKind.Local).AddTicks(9001), new Guid("03879e23-1be2-44cb-8ee1-b9fab871a452") });
        }
    }
}
