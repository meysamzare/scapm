using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "Logs");

            // migrationBuilder.RenameColumn(
            //     name: "agnetType",
            //     table: "SystemLogs",
            //     newName: "dateString");

            // migrationBuilder.AlterColumn<long>(
            //     name: "Id",
            //     table: "SystemLogs",
            //     nullable: false,
            //     oldClrType: typeof(Guid))
            //     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            // migrationBuilder.AddColumn<string>(
            //     name: "DeleteObjects",
            //     table: "SystemLogs",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Object",
            //     table: "SystemLogs",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "OldObject",
            //     table: "SystemLogs",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "ResponseData",
            //     table: "SystemLogs",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Table",
            //     table: "SystemLogs",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "TableObjectIds",
            //     table: "SystemLogs",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Type",
            //     table: "SystemLogs",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "agentType",
            //     table: "SystemLogs",
            //     nullable: true);



            // migrationBuilder.CreateTable(
            //     name: "ILogSystem",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(nullable: false)
            //             .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //         Type = table.Column<string>(nullable: true),
            //         Date = table.Column<DateTime>(nullable: false),
            //         Desc = table.Column<string>(nullable: true),
            //         Event = table.Column<string>(nullable: true),
            //         Ip = table.Column<string>(nullable: true),
            //         LogSource = table.Column<string>(nullable: true),
            //         agentId = table.Column<int>(nullable: false),
            //         agentName = table.Column<string>(nullable: true),
            //         agnetType = table.Column<string>(nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Logs", x => x.Id);
            //     });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 6, 4, 12, 52, 25, 677, DateTimeKind.Local).AddTicks(134), new Guid("6d8e30fc-c82d-4e83-bb5c-b62fa9e5ca42") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "DeleteObjects",
            //     table: "SystemLogs");

            // migrationBuilder.DropColumn(
            //     name: "Object",
            //     table: "SystemLogs");

            // migrationBuilder.DropColumn(
            //     name: "OldObject",
            //     table: "SystemLogs");

            // migrationBuilder.DropColumn(
            //     name: "ResponseData",
            //     table: "SystemLogs");

            // migrationBuilder.DropColumn(
            //     name: "Table",
            //     table: "SystemLogs");

            // migrationBuilder.DropColumn(
            //     name: "TableObjectIds",
            //     table: "SystemLogs");

            // migrationBuilder.DropColumn(
            //     name: "Type",
            //     table: "SystemLogs");

            // migrationBuilder.DropColumn(
            //     name: "agentType",
            //     table: "SystemLogs");

            // migrationBuilder.RenameColumn(
            //     name: "dateString",
            //     table: "SystemLogs",
            //     newName: "agnetType");

            // // migrationBuilder.AlterColumn<Guid>(
            // //     name: "Id",
            // //     table: "SystemLogs",
            // //     nullable: false,
            // //     oldClrType: typeof(long))
            // //     .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            // migrationBuilder.CreateTable(
            //     name: "Logs",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(nullable: false)
            //             .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //         Date = table.Column<DateTime>(nullable: false),
            //         Desc = table.Column<string>(nullable: true),
            //         Event = table.Column<string>(nullable: true),
            //         Ip = table.Column<string>(nullable: true),
            //         LogSource = table.Column<string>(nullable: true),
            //         agentId = table.Column<int>(nullable: false),
            //         agentName = table.Column<string>(nullable: true),
            //         agnetType = table.Column<string>(nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Logs", x => x.Id);
            //     });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 5, 26, 0, 19, 48, 401, DateTimeKind.Local).AddTicks(2332), new Guid("4a4e56ec-22c3-4de6-8ae0-ce477426cc42") });
        }
    }
}
