using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate21_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostUrl",
                table: "Ind.MainSlideShow");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 4, 14, 2, 6, 833, DateTimeKind.Local).AddTicks(293), new Guid("e907d88d-5f0f-4ad9-8ce6-fb8b5c11db9d") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostUrl",
                table: "Ind.MainSlideShow",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 4, 0, 23, 9, 182, DateTimeKind.Local).AddTicks(8162), new Guid("dd02f365-4ff6-415d-83d6-7b6ed55fbd4c") });
        }
    }
}
