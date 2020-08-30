using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate44_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowScoreAfterDone",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 8, 12, 12, 27, 21, 916, DateTimeKind.Local).AddTicks(8012), new Guid("a69a5d8e-197f-4a21-a756-5fd73c2f7bb8") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowScoreAfterDone",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 7, 9, 14, 44, 12, 879, DateTimeKind.Local).AddTicks(6900), new Guid("589907fa-017c-4bf8-9ee8-824f0c5b007d") });
        }
    }
}
