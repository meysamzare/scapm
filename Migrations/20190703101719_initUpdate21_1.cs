using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate21_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HeaderPic",
                table: "Ind.Post",
                newName: "HeaderPicUrl");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 3, 14, 47, 18, 929, DateTimeKind.Local).AddTicks(4751), new Guid("6d5d4ce3-1ea7-43d2-abdf-dfadcf433588") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HeaderPicUrl",
                table: "Ind.Post",
                newName: "HeaderPic");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 3, 12, 23, 15, 128, DateTimeKind.Local).AddTicks(120), new Guid("2daa4af2-64f2-4dbf-ab27-e463acff0ca7") });
        }
    }
}
