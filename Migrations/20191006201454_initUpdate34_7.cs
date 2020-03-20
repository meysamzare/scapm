using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate34_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 6, 23, 44, 52, 739, DateTimeKind.Local).AddTicks(2142), new Guid("0885041e-0c9e-48b4-9ef3-790e37d25832") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 6, 23, 39, 52, 124, DateTimeKind.Local).AddTicks(4842), new Guid("9d962916-c4a6-4565-93ef-0a26c9415f95") });
        }
    }
}
