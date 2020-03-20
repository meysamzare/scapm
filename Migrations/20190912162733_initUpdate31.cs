using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_Picture",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_PictureGallery",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Picture",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_PictureGallery",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Picture",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_PictureGallery",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Picture",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_PictureGallery",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Ind.PictureGallery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Like = table.Column<int>(nullable: false),
                    DisLike = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ind.PictureGallery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ind.Picture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    PictureGalleryId = table.Column<int>(nullable: false),
                    PicUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ind.Picture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ind.Picture_Ind.PictureGallery_PictureGalleryId",
                        column: x => x.PictureGalleryId,
                        principalTable: "Ind.PictureGallery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 12, 20, 57, 32, 643, DateTimeKind.Local).AddTicks(2967), new Guid("cb5ca89d-94f2-44be-a095-d113c486b899") });

            migrationBuilder.CreateIndex(
                name: "IX_Ind.Picture_PictureGalleryId",
                table: "Ind.Picture",
                column: "PictureGalleryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ind.Picture");

            migrationBuilder.DropTable(
                name: "Ind.PictureGallery");

            migrationBuilder.DropColumn(
                name: "Add_Picture",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_PictureGallery",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Picture",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_PictureGallery",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Picture",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_PictureGallery",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Picture",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_PictureGallery",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 9, 10, 13, 35, 31, 674, DateTimeKind.Local).AddTicks(725), new Guid("5a74991d-5876-41ba-9bed-4655aaf06c45") });
        }
    }
}
