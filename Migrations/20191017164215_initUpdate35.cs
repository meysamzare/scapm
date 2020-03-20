using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ind.Picture_Ind.PictureGallery_PictureGalleryId",
                table: "Ind.Picture");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Logs",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "PictureGalleryId",
                table: "Ind.Picture",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Ind.Picture",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 17, 20, 12, 14, 200, DateTimeKind.Local).AddTicks(1674), new Guid("266ce3a3-7217-494b-a28f-e59249cc59f8") });

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.Picture_Ind.PictureGallery_PictureGalleryId",
                table: "Ind.Picture",
                column: "PictureGalleryId",
                principalTable: "Ind.PictureGallery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ind.Picture_Ind.PictureGallery_PictureGalleryId",
                table: "Ind.Picture");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Ind.Picture");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Logs",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "PictureGalleryId",
                table: "Ind.Picture",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 6, 23, 44, 52, 739, DateTimeKind.Local).AddTicks(2142), new Guid("0885041e-0c9e-48b4-9ef3-790e37d25832") });

            migrationBuilder.AddForeignKey(
                name: "FK_Ind.Picture_Ind.PictureGallery_PictureGalleryId",
                table: "Ind.Picture",
                column: "PictureGalleryId",
                principalTable: "Ind.PictureGallery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
