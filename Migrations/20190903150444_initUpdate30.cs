using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_Notification",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Notification",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Notification",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Notification",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "NotificationAgents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubscribeDate = table.Column<DateTime>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    IsParent = table.Column<bool>(nullable: false),
                    Endpoint = table.Column<string>(nullable: true),
                    P256DH = table.Column<string>(nullable: true),
                    Auth = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationAgents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationAgents_sm.Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "sm.Student",
                        principalColumn: "Std.Autonum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    ShortContent = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ButtonTitle = table.Column<string>(nullable: true),
                    SendDate = table.Column<DateTime>(nullable: false),
                    NotiifcationType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 3, 19, 34, 43, 127, DateTimeKind.Local).AddTicks(9546), new Guid("549609d3-cdba-4430-a29a-22765aadcaac") });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationAgents_StudentId",
                table: "NotificationAgents",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationAgents");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropColumn(
                name: "Add_Notification",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Notification",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Notification",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Notification",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 30, 11, 52, 0, 631, DateTimeKind.Local).AddTicks(1572), new Guid("bbd25597-4830-430a-94e5-f05fec03d566") });
        }
    }
}
