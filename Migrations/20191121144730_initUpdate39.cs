using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationSeens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NotificationId = table.Column<int>(nullable: false),
                    AgentName = table.Column<string>(nullable: true),
                    AgentId = table.Column<int>(nullable: false),
                    AgentType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSeens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSeens_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 21, 18, 17, 29, 404, DateTimeKind.Local).AddTicks(1808), new Guid("a60f0902-1f62-46e3-b706-01a8d2c1635a") });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSeens_NotificationId",
                table: "NotificationSeens",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationSeens");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 11, 20, 9, 21, 37, 399, DateTimeKind.Local).AddTicks(3877), new Guid("85dbb9e2-1a14-4bbc-bf4b-5dffcb0e5784") });
        }
    }
}
