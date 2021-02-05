using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate48_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StdClassMngId",
                table: "StdPayments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 1, 18, 21, 36, 55, 54, DateTimeKind.Local).AddTicks(4949), new Guid("53c582ae-9725-4591-9bac-2af0a101c444") });

            migrationBuilder.CreateIndex(
                name: "IX_StdPayments_StdClassMngId",
                table: "StdPayments",
                column: "StdClassMngId");

            migrationBuilder.AddForeignKey(
                name: "FK_StdPayments_StdClassMngs_StdClassMngId",
                table: "StdPayments",
                column: "StdClassMngId",
                principalTable: "StdClassMngs",
                principalColumn: "StdClassMng.Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StdPayments_StdClassMngs_StdClassMngId",
                table: "StdPayments");

            migrationBuilder.DropIndex(
                name: "IX_StdPayments_StdClassMngId",
                table: "StdPayments");

            migrationBuilder.DropColumn(
                name: "StdClassMngId",
                table: "StdPayments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 1, 1, 21, 50, 0, 994, DateTimeKind.Local).AddTicks(4271), new Guid("71fa575d-bc4b-485d-bf7c-3325e3c7106b") });
        }
    }
}
