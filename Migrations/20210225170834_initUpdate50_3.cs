using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate50_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "Categories",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 25, 20, 38, 33, 313, DateTimeKind.Local).AddTicks(767), new Guid("1d5c2612-fd51-4828-a1a4-48cfb6ddeb95") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GId",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 21, 20, 19, 23, 146, DateTimeKind.Local).AddTicks(3591), new Guid("40949c3b-09a4-43b1-b6bf-94cedc04d224") });
        }
    }
}
