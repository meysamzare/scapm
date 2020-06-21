using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate42_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.Questions_sm.OnlineExamCategory_QuestionCategoryId",
                table: "sm.Questions");

            migrationBuilder.DropTable(
                name: "sm.OnlineExamCategory");

            migrationBuilder.DropIndex(
                name: "IX_sm.Questions_QuestionCategoryId",
                table: "sm.Questions");

            migrationBuilder.DropColumn(
                name: "QuestionCategoryId",
                table: "sm.Questions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 5, 17, 11, 21, 38, 426, DateTimeKind.Local).AddTicks(8941), new Guid("426afadc-085a-41b3-93d9-eb4f3806afb4") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionCategoryId",
                table: "sm.Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "sm.OnlineExamCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.OnlineExamCategory", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 5, 17, 4, 25, 29, 316, DateTimeKind.Local).AddTicks(7237), new Guid("825a7af2-cb8f-4014-9580-c6c360534c0c") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.Questions_QuestionCategoryId",
                table: "sm.Questions",
                column: "QuestionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Questions_sm.OnlineExamCategory_QuestionCategoryId",
                table: "sm.Questions",
                column: "QuestionCategoryId",
                principalTable: "sm.OnlineExamCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
