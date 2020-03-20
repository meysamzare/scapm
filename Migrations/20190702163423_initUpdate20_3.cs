using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate20_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegisterPicUrl",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowInfoPicUrl",
                table: "Categories",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 2, 21, 4, 22, 875, DateTimeKind.Local).AddTicks(6818), new Guid("1fbc21ad-9a97-4155-8558-317f401ad0af") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterPicUrl",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ShowInfoPicUrl",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 1, 11, 4, 25, 867, DateTimeKind.Local).AddTicks(5734), new Guid("019081f9-d079-4b22-9caa-1e9a7711aafa") });
        }
    }
}
