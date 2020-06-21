using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate43_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_SystemLogs",
            //     table: "SystemLogs");

            // migrationBuilder.RenameTable(
            //     name: "SystemLogs",
            //     newName: "SysLogs");

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_SysLogs",
            //     table: "SysLogs",
            //     column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 6, 4, 13, 1, 2, 653, DateTimeKind.Local).AddTicks(2206), new Guid("5d679618-34c0-46ab-9f36-3ca99b462394") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_SysLogs",
            //     table: "SysLogs");

            // migrationBuilder.RenameTable(
            //     name: "SysLogs",
            //     newName: "SystemLogs");

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_SystemLogs",
            //     table: "SystemLogs",
            //     column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 6, 4, 12, 52, 25, 677, DateTimeKind.Local).AddTicks(134), new Guid("6d8e30fc-c82d-4e83-bb5c-b62fa9e5ca42") });
        }
    }
}
