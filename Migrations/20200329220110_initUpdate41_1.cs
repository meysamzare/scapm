using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate41_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxFileSize",
                table: "Attributes",
                nullable: false,
                defaultValue: 15);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 3, 30, 2, 31, 9, 335, DateTimeKind.Local).AddTicks(2645), new Guid("d46c99a4-68b5-4aa6-b986-c07cb8927775") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxFileSize",
                table: "Attributes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 3, 8, 12, 41, 32, 792, DateTimeKind.Local).AddTicks(3753), new Guid("b018e648-a445-4900-b669-82db02a1803e") });
        }
    }
}
