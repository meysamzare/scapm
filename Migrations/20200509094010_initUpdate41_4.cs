using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate41_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionCategoryId",
                table: "sm.Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "Attributes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "AttributeOptions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    IsTrue = table.Column<bool>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeOptions_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                values: new object[] { new DateTime(2020, 5, 9, 14, 10, 9, 418, DateTimeKind.Local).AddTicks(4937), new Guid("0030f7a0-c228-4dc8-b91a-8b0d7d0c744e") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.Questions_QuestionCategoryId",
                table: "sm.Questions",
                column: "QuestionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeOptions_AttributeId",
                table: "AttributeOptions",
                column: "AttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Questions_sm.OnlineExamCategory_QuestionCategoryId",
                table: "sm.Questions",
                column: "QuestionCategoryId",
                principalTable: "sm.OnlineExamCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sm.Questions_sm.OnlineExamCategory_QuestionCategoryId",
                table: "sm.Questions");

            migrationBuilder.DropTable(
                name: "AttributeOptions");

            migrationBuilder.DropTable(
                name: "sm.OnlineExamCategory");

            migrationBuilder.DropIndex(
                name: "IX_sm.Questions_QuestionCategoryId",
                table: "sm.Questions");

            migrationBuilder.DropColumn(
                name: "QuestionCategoryId",
                table: "sm.Questions");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Attributes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 4, 22, 16, 57, 41, 323, DateTimeKind.Local).AddTicks(7271), new Guid("a01690b3-0a91-4566-bd26-620f0525fb1d") });
        }
    }
}
