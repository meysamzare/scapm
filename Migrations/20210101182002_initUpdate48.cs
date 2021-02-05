using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate48 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisterItemLogins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    CategoryAuthorizeState = table.Column<string>(nullable: true),
                    UserType = table.Column<string>(nullable: true),
                    GradeId = table.Column<int>(nullable: false),
                    ClassId = table.Column<int>(nullable: false),
                    IP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterItemLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisterItemLogins_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2021, 1, 1, 21, 50, 0, 994, DateTimeKind.Local).AddTicks(4271), new Guid("71fa575d-bc4b-485d-bf7c-3325e3c7106b") });

            migrationBuilder.CreateIndex(
                name: "IX_RegisterItemLogins_CategoryId",
                table: "RegisterItemLogins",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterItemLogins");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 12, 31, 20, 19, 57, 293, DateTimeKind.Local).AddTicks(2450), new Guid("385adccd-5c71-456e-8ada-5acbe24c3ef8") });
        }
    }
}
