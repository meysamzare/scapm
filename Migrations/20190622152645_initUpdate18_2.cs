using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate18_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "haveRequestToReview",
                table: "Tbl_WorkBook_Details",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 22, 19, 56, 45, 17, DateTimeKind.Local).AddTicks(14), new Guid("58c95686-ad8f-4222-b9a0-1a6022469d87") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "haveRequestToReview",
                table: "Tbl_WorkBook_Details");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 21, 16, 7, 25, 598, DateTimeKind.Local).AddTicks(2324), new Guid("0ba4101b-bf97-4ee1-8423-096d1cc8bbe9") });
        }
    }
}
