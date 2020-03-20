using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Event = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    agentId = table.Column<int>(nullable: false),
                    agnetType = table.Column<string>(nullable: true),
                    agentName = table.Column<string>(nullable: true),
                    LogSource = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLogs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 20, 9, 21, 37, 399, DateTimeKind.Local).AddTicks(3877), new Guid("85dbb9e2-1a14-4bbc-bf4b-5dffcb0e5784") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemLogs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 18, 10, 4, 31, 235, DateTimeKind.Local).AddTicks(8749), new Guid("56a43810-5d94-49a4-b5cd-5063f2abb6ff") });
        }
    }
}
