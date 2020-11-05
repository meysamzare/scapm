using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate46_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Units_UnitId",
                table: "Items");

            // migrationBuilder.EnsureSchema(
            //     name: "tahaschi_meysam");

            // migrationBuilder.RenameTable(
            //     name: "sm.Salary",
            //     newName: "sm.Salary",
            //     newSchema: "tahaschi_meysam");

            migrationBuilder.AddColumn<int>(
                name: "ScoreType",
                table: "sm.Yeareducation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DescriptiveScoreId",
                table: "sm.ExamScore",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Items",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "DescriptiveScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    EnName = table.Column<string>(nullable: true),
                    FromNumber = table.Column<double>(nullable: false),
                    ToNumber = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescriptiveScores", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 10, 22, 18, 33, 49, 320, DateTimeKind.Local).AddTicks(642), new Guid("e3980253-beae-44d6-a381-4f94ff642d4e") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.ExamScore_DescriptiveScoreId",
                table: "sm.ExamScore",
                column: "DescriptiveScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Units_UnitId",
                table: "Items",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.ExamScore_DescriptiveScores_DescriptiveScoreId",
                table: "sm.ExamScore",
                column: "DescriptiveScoreId",
                principalTable: "DescriptiveScores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Units_UnitId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_sm.ExamScore_DescriptiveScores_DescriptiveScoreId",
                table: "sm.ExamScore");

            migrationBuilder.DropTable(
                name: "DescriptiveScores");

            migrationBuilder.DropIndex(
                name: "IX_sm.ExamScore_DescriptiveScoreId",
                table: "sm.ExamScore");

            migrationBuilder.DropColumn(
                name: "ScoreType",
                table: "sm.Yeareducation");

            migrationBuilder.DropColumn(
                name: "DescriptiveScoreId",
                table: "sm.ExamScore");

            // migrationBuilder.RenameTable(
            //     name: "sm.Salary",
            //     schema: "tahaschi_meysam",
            //     newName: "sm.Salary");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 9, 30, 19, 54, 26, 687, DateTimeKind.Local).AddTicks(9529), new Guid("9d9e7b75-eca0-4133-8880-b7eeb7ac7ed0") });

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Units_UnitId",
                table: "Items",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
