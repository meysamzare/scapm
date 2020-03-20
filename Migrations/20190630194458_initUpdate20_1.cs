using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate20_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartyContractName",
                table: "Fin.Contract",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 1, 0, 14, 57, 823, DateTimeKind.Local).AddTicks(4618), new Guid("723a1fe3-d2f2-4a7e-b644-0ea9fdf70771") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartyContractName",
                table: "Fin.Contract");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 30, 20, 6, 57, 370, DateTimeKind.Local).AddTicks(2996), new Guid("7d4fb1e3-a6ed-4c00-87a1-8e557a9e5c7c") });
        }
    }
}
