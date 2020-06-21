using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate43_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "SysLogs");

            migrationBuilder.CreateTable(
                name: "ILogSystems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    dateString = table.Column<string>(nullable: true),
                    agentId = table.Column<int>(nullable: false),
                    agentType = table.Column<string>(nullable: true),
                    agentName = table.Column<string>(nullable: true),
                    Object = table.Column<string>(nullable: true),
                    OldObject = table.Column<string>(nullable: true),
                    DeleteObjects = table.Column<string>(nullable: true),
                    Table = table.Column<string>(nullable: true),
                    TableObjectIds = table.Column<string>(nullable: true),
                    ResponseData = table.Column<string>(nullable: true),
                    LogSource = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Event = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ILogSystems", x => x.Id);
                });

            // migrationBuilder.CreateTable(
            //     name: "Logs",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(nullable: false)
            //             .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //         Event = table.Column<string>(nullable: true),
            //         Desc = table.Column<string>(nullable: true),
            //         Ip = table.Column<string>(nullable: true),
            //         Date = table.Column<DateTime>(nullable: false),
            //         agentId = table.Column<int>(nullable: false),
            //         agnetType = table.Column<string>(nullable: true),
            //         agentName = table.Column<string>(nullable: true),
            //         LogSource = table.Column<string>(nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Logs", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "SystemLogs",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(nullable: false),
            //         Event = table.Column<string>(nullable: true),
            //         Desc = table.Column<string>(nullable: true),
            //         Ip = table.Column<string>(nullable: true),
            //         Date = table.Column<DateTime>(nullable: false),
            //         agentId = table.Column<int>(nullable: false),
            //         agnetType = table.Column<string>(nullable: true),
            //         agentName = table.Column<string>(nullable: true),
            //         LogSource = table.Column<string>(nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_SystemLogs", x => x.Id);
            //     });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 6, 4, 13, 39, 44, 341, DateTimeKind.Local).AddTicks(9141), new Guid("dbadbfed-d5ce-4bd4-a454-6e62a8126eb4") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ILogSystems");

            // migrationBuilder.DropTable(
            //     name: "Logs");

            // migrationBuilder.DropTable(
            //     name: "SystemLogs");

            // migrationBuilder.CreateTable(
            //     name: "SysLogs",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(nullable: false)
            //             .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //         Date = table.Column<DateTime>(nullable: false),
            //         DeleteObjects = table.Column<string>(nullable: true),
            //         Desc = table.Column<string>(nullable: true),
            //         Event = table.Column<string>(nullable: true),
            //         Ip = table.Column<string>(nullable: true),
            //         LogSource = table.Column<string>(nullable: true),
            //         Object = table.Column<string>(nullable: true),
            //         OldObject = table.Column<string>(nullable: true),
            //         ResponseData = table.Column<string>(nullable: true),
            //         Table = table.Column<string>(nullable: true),
            //         TableObjectIds = table.Column<string>(nullable: true),
            //         Type = table.Column<string>(nullable: true),
            //         agentId = table.Column<int>(nullable: false),
            //         agentName = table.Column<string>(nullable: true),
            //         agentType = table.Column<string>(nullable: true),
            //         dateString = table.Column<string>(nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_SysLogs", x => x.Id);
            //     });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 6, 4, 13, 1, 2, 653, DateTimeKind.Local).AddTicks(2206), new Guid("5d679618-34c0-46ab-9f36-3ca99b462394") });
        }
    }
}
