using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate25_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ind.Comment_Ind.Comment_ParentId",
                table: "Ind.Comment");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Ind.Comment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 17, 1, 0, 18, 262, DateTimeKind.Local).AddTicks(9025), new Guid("5e744403-ec66-4fbe-81e4-256c17477c92") });

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.Comment_Ind.Comment_ParentId",
                table: "Ind.Comment",
                column: "ParentId",
                principalTable: "Ind.Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ind.Comment_Ind.Comment_ParentId",
                table: "Ind.Comment");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Ind.Comment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 14, 23, 52, 3, 101, DateTimeKind.Local).AddTicks(2164), new Guid("a6ab64b6-5946-426d-a224-848130ce3291") });

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.Comment_Ind.Comment_ParentId",
                table: "Ind.Comment",
                column: "ParentId",
                principalTable: "Ind.Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
