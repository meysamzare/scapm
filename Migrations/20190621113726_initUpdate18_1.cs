using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate18_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "state",
                table: "Tbl_WorkBook_Header",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 21, 16, 7, 25, 598, DateTimeKind.Local).AddTicks(2324), new Guid("0ba4101b-bf97-4ee1-8423-096d1cc8bbe9") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "state",
                table: "Tbl_WorkBook_Header");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 21, 15, 59, 25, 173, DateTimeKind.Local).AddTicks(911), new Guid("c05de872-4dc5-4f93-ab15-56cdf0f690b0") });
        }
    }
}
