using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate37_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePic",
                table: "Users",
                newName: "PicUrl");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 18, 10, 4, 31, 235, DateTimeKind.Local).AddTicks(8749), new Guid("56a43810-5d94-49a4-b5cd-5063f2abb6ff") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PicUrl",
                table: "Users",
                newName: "ProfilePic");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 15, 0, 29, 59, 279, DateTimeKind.Local).AddTicks(1623), new Guid("59a19592-7d5c-47f7-b7c4-abfcfed18c45") });
        }
    }
}
