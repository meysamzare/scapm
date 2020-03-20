using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_BestStudent",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_BestStudent",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_BestStudent",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_BestStudent",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Ind.BestStudent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    PicUrl = table.Column<string>(nullable: true),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ind.BestStudent", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 26, 16, 14, 28, 431, DateTimeKind.Local).AddTicks(9230), new Guid("06923c76-53fe-47f2-bc41-330db57ab5ed") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ind.BestStudent");

            migrationBuilder.DropColumn(
                name: "Add_BestStudent",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_BestStudent",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_BestStudent",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_BestStudent",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 25, 22, 51, 19, 724, DateTimeKind.Local).AddTicks(7842), new Guid("0b7d36d6-892c-4ff8-8a78-f1a923777501") });
        }
    }
}
