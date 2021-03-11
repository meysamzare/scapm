using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate50_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniqLimitCount",
                table: "Attributes",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "UniqLimitCount",
                table: "AttributeOptions",
                nullable: true,
                defaultValue: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 3, 1, 22, 47, 0, 750, DateTimeKind.Local).AddTicks(726), new Guid("0712dd51-7dc8-4afd-a30c-b712246cc3fc") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqLimitCount",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UniqLimitCount",
                table: "AttributeOptions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 25, 20, 38, 33, 313, DateTimeKind.Local).AddTicks(767), new Guid("1d5c2612-fd51-4828-a1a4-48cfb6ddeb95") });
        }
    }
}
