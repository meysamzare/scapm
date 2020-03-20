using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate21_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Ind.MainSlideShow",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 4, 0, 23, 9, 182, DateTimeKind.Local).AddTicks(8162), new Guid("dd02f365-4ff6-415d-83d6-7b6ed55fbd4c") });

            migrationBuilder.CreateIndex(
                name: "IX_Ind.MainSlideShow_PostId",
                table: "Ind.MainSlideShow",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.MainSlideShow_Ind.Post_PostId",
                table: "Ind.MainSlideShow",
                column: "PostId",
                principalTable: "Ind.Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ind.MainSlideShow_Ind.Post_PostId",
                table: "Ind.MainSlideShow");

            migrationBuilder.DropIndex(
                name: "IX_Ind.MainSlideShow_PostId",
                table: "Ind.MainSlideShow");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Ind.MainSlideShow");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 3, 14, 47, 18, 929, DateTimeKind.Local).AddTicks(4751), new Guid("6d5d4ce3-1ea7-43d2-abdf-dfadcf433588") });
        }
    }
}
