using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate41_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HaveExternalUrl",
                table: "Links",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 4, 4, 22, 23, 59, 208, DateTimeKind.Local).AddTicks(9984), new Guid("c6c86fba-6330-49a2-8943-1384b36afc74") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HaveExternalUrl",
                table: "Links");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 3, 30, 2, 31, 9, 335, DateTimeKind.Local).AddTicks(2645), new Guid("d46c99a4-68b5-4aa6-b986-c07cb8927775") });
        }
    }
}
