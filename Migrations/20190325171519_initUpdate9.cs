using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_Yeareducation",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Yeareducation",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Yeareducation",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Yeareducation",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "sm.Yeareducation",
                columns: table => new
                {
                    eduYeareduCode = table.Column<int>(name: "edu.YeareduCode", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    eduYeareduName = table.Column<string>(name: "edu.YeareduName", nullable: true),
                    eduDateStart = table.Column<DateTime>(name: "edu.DateStart", nullable: false),
                    eduDateEnd = table.Column<DateTime>(name: "edu.DateEnd", nullable: false),
                    eduDesc = table.Column<string>(name: "edu.Desc", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.Yeareducation", x => x.eduYeareduCode);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GrdAutonumber = table.Column<int>(name: "Grd.Autonumber", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrdId = table.Column<string>(name: "Grd.Id", nullable: true),
                    GrdName = table.Column<string>(name: "Grd.Name", nullable: true),
                    GrdinsCode = table.Column<int>(name: "Grd.insCode", nullable: false),
                    GrdeduyearCode = table.Column<int>(name: "Grd.eduyearCode", nullable: false),
                    GrdCapasity = table.Column<int>(name: "Grd.Capasity", nullable: false),
                    GrdOrgCode = table.Column<string>(name: "Grd.OrgCode", nullable: true),
                    GrdInternalCode = table.Column<string>(name: "Grd.InternalCode", nullable: true),
                    GrdDesc = table.Column<string>(name: "Grd.Desc", nullable: true),
                    GrdOrder = table.Column<int>(name: "Grd.Order", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GrdAutonumber);
                    table.ForeignKey(
                        name: "FK_Grades_sm.insTitute_Grd.insCode",
                        column: x => x.GrdinsCode,
                        principalTable: "sm.insTitute",
                        principalColumn: "ins.AutoNum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_sm.Yeareducation_Grd.eduyearCode",
                        column: x => x.GrdeduyearCode,
                        principalTable: "sm.Yeareducation",
                        principalColumn: "edu.YeareduCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClsAutonumber = table.Column<int>(name: "Cls.Autonumber", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClsId = table.Column<int>(name: "Cls.Id", nullable: false),
                    ClsName = table.Column<string>(name: "Cls.Name", nullable: true),
                    ClsSection = table.Column<string>(name: "Cls.Section", nullable: true),
                    GrdId = table.Column<int>(name: "Grd.Id", nullable: false),
                    ClsCapasity = table.Column<int>(name: "Cls.Capasity", nullable: false),
                    ClsOrder = table.Column<int>(name: "Cls.Order", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClsAutonumber);
                    table.ForeignKey(
                        name: "FK_Classes_Grades_Grd.Id",
                        column: x => x.GrdId,
                        principalTable: "Grades",
                        principalColumn: "Grd.Autonumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 25, 21, 45, 18, 761, DateTimeKind.Local).AddTicks(2629), new Guid("c073f383-0c7b-47b5-8e37-0d9cac7dd826") });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Grd.Id",
                table: "Classes",
                column: "Grd.Id");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_Grd.insCode",
                table: "Grades",
                column: "Grd.insCode");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_Grd.eduyearCode",
                table: "Grades",
                column: "Grd.eduyearCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "sm.Yeareducation");

            migrationBuilder.DropColumn(
                name: "Add_Yeareducation",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Yeareducation",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Yeareducation",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Yeareducation",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 3, 22, 20, 46, 13, 9, DateTimeKind.Local).AddTicks(4729), new Guid("a39296bb-4f8b-416a-bab7-c1fb7f52e013") });
        }
    }
}
