using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentTypeId",
                table: "sm.StdClassMng",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentType", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 5, 18, 2, 29, 474, DateTimeKind.Local).AddTicks(3796), new Guid("1571669b-ad18-438c-9bfe-eb9a6fe863b7") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.StdClassMng_StudentTypeId",
                table: "sm.StdClassMng",
                column: "StudentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_sm.StdClassMng_StudentType_StudentTypeId",
                table: "sm.StdClassMng",
                column: "StudentTypeId",
                principalTable: "StudentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.StdClassMng_StudentType_StudentTypeId",
                table: "sm.StdClassMng");

            migrationBuilder.DropTable(
                name: "StudentType");

            migrationBuilder.DropIndex(
                name: "IX_sm.StdClassMng_StudentTypeId",
                table: "sm.StdClassMng");

            migrationBuilder.DropColumn(
                name: "StudentTypeId",
                table: "sm.StdClassMng");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 1, 13, 1, 33, 501, DateTimeKind.Local).AddTicks(9094), new Guid("9baebdf2-0678-4a1c-84a0-386c36aa1d98") });
        }
    }
}
