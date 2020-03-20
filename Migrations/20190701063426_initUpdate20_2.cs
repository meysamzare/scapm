using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate20_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 1, 11, 4, 25, 867, DateTimeKind.Local).AddTicks(5734), new Guid("019081f9-d079-4b22-9caa-1e9a7711aafa") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 1, 0, 14, 57, 823, DateTimeKind.Local).AddTicks(4618), new Guid("723a1fe3-d2f2-4a7e-b644-0ea9fdf70771") });
        }
    }
}
