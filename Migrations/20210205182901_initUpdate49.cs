using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_OnlineClassServer",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_OnlineClassServer",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_OnlineClassServer",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_OnlineClassServer",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OnlineClassServerId",
                table: "OnlineClasses",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateTable(
                name: "OnlineClassServers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    PrivateKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineClassServers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 2, 5, 21, 59, 1, 410, DateTimeKind.Local).AddTicks(6401), new Guid("e080c600-d9d0-419b-a786-0cd79cca86e0") });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClasses_OnlineClassServerId",
                table: "OnlineClasses",
                column: "OnlineClassServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_OnlineClassServers_OnlineClassServerId",
                table: "OnlineClasses",
                column: "OnlineClassServerId",
                principalTable: "OnlineClassServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_OnlineClassServers_OnlineClassServerId",
                table: "OnlineClasses");

            migrationBuilder.DropTable(
                name: "OnlineClassServers");

            migrationBuilder.DropIndex(
                name: "IX_OnlineClasses_OnlineClassServerId",
                table: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "Add_OnlineClassServer",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_OnlineClassServer",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_OnlineClassServer",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_OnlineClassServer",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "OnlineClassServerId",
                table: "OnlineClasses");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 1, 18, 21, 36, 55, 54, DateTimeKind.Local).AddTicks(4949), new Guid("53c582ae-9725-4591-9bac-2af0a101c444") });
        }
    }
}
