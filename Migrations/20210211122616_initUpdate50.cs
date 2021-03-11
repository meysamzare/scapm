using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineClassLogins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OnlineClassId = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    AgentType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineClassLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineClassLogins_OnlineClasses_OnlineClassId",
                        column: x => x.OnlineClassId,
                        principalTable: "OnlineClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 11, 15, 56, 16, 133, DateTimeKind.Local).AddTicks(4250), new Guid("72b76f38-813f-46a9-95f1-af72b054b20e") });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClassLogins_OnlineClassId",
                table: "OnlineClassLogins",
                column: "OnlineClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlineClassLogins");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 6, 22, 7, 34, 304, DateTimeKind.Local).AddTicks(9679), new Guid("15d1812e-5c75-4b16-b4f7-7aab8f58696c") });
        }
    }
}
