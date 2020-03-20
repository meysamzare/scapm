using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate32_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MobileChats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderType = table.Column<int>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    SenderName = table.Column<string>(nullable: true),
                    ReciverType = table.Column<int>(nullable: false),
                    ReciverId = table.Column<int>(nullable: false),
                    ReciverName = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    SendDate = table.Column<DateTime>(nullable: false),
                    ReciveDate = table.Column<DateTime>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileChats", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 19, 21, 51, 33, 327, DateTimeKind.Local).AddTicks(218), new Guid("687a5eb9-aae3-4125-8b65-00b052d3e864") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileChats");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 18, 20, 55, 27, 145, DateTimeKind.Local).AddTicks(560), new Guid("210c99ee-2dbd-46ca-806b-279b933e48d9") });
        }
    }
}
