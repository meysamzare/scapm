using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_InsTitute",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_InsTitute",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_InsTitute",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_InsTitute",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "sm.insTitute",
                columns: table => new
                {
                    insAutoNum = table.Column<int>(name: "ins.AutoNum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    insCode = table.Column<int>(name: "ins.Code", nullable: true),
                    insName = table.Column<string>(name: "ins.Name", nullable: true),
                    insOrgCode = table.Column<int>(name: "ins.OrgCode", nullable: false),
                    insOrgSection = table.Column<string>(name: "ins.OrgSection", nullable: true),
                    insOrgSex = table.Column<int>(name: "ins.OrgSex", nullable: false),
                    insState = table.Column<string>(name: "ins.State", nullable: true),
                    insCity = table.Column<string>(name: "ins.City", nullable: true),
                    insAddress = table.Column<string>(name: "ins.Address", nullable: true),
                    insPostCode = table.Column<string>(name: "ins.PostCode", nullable: true),
                    insTell = table.Column<string>(name: "ins.Tell", nullable: true),
                    insEmail = table.Column<string>(name: "ins.Email", nullable: true),
                    insDesc = table.Column<string>(name: "ins.Desc", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.insTitute", x => x.insAutoNum);
                    table.ForeignKey(
                        name: "FK_sm.insTitute_sm.insTitute_ins.Code",
                        column: x => x.insCode,
                        principalTable: "sm.insTitute",
                        principalColumn: "ins.AutoNum",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 22, 20, 46, 13, 9, DateTimeKind.Local).AddTicks(4729), new Guid("a39296bb-4f8b-416a-bab7-c1fb7f52e013") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.insTitute_ins.Code",
                table: "sm.insTitute",
                column: "ins.Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sm.insTitute");

            migrationBuilder.DropColumn(
                name: "Add_InsTitute",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_InsTitute",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_InsTitute",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_InsTitute",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 18, 12, 56, 31, 662, DateTimeKind.Local).AddTicks(4226), new Guid("1ebf656b-a9db-4eb9-bb78-09492cce4411") });
        }
    }
}
