using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "post_amoozesh",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_dabirKhaneBargozidegan",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_daneshAmookhtegan",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_daneshAmoozan",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_darbareTaha",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_darkhastTajdidNazar",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_ehrazeHoviat",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_enteghadVaPishnahad",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_enzebati",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_fadak",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_faq",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_feed",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_forsatHayeShoghli",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_ghesmathayeSamane",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_hedayatTahsil",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_it",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_mali",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_mokatebat",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_moshaver",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_parvaresh",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_post",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_sabteNam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_sharayetSabteNam",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_tamasBaTaha",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "post_voroodBeSystem",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 10, 22, 28, 26, 671, DateTimeKind.Local).AddTicks(2054), new Guid("255e2ab2-5945-4a8c-9bd2-5be0224056bc") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "post_amoozesh",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_dabirKhaneBargozidegan",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_daneshAmookhtegan",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_daneshAmoozan",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_darbareTaha",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_darkhastTajdidNazar",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_ehrazeHoviat",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_enteghadVaPishnahad",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_enzebati",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_fadak",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_faq",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_feed",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_forsatHayeShoghli",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_ghesmathayeSamane",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_hedayatTahsil",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_it",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_mali",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_mokatebat",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_moshaver",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_parvaresh",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_post",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_sabteNam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_sharayetSabteNam",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_tamasBaTaha",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "post_voroodBeSystem",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 8, 8, 24, 26, 660, DateTimeKind.Local).AddTicks(4428), new Guid("7965bede-e2c7-4147-bcd9-1be61287b3ac") });
        }
    }
}
