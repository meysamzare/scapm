using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_Comment",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_MainSlideShow",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Post",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Schedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Comment",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_MainSlideShow",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Post",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Schedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Comment",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_MainSlideShow",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Post",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Schedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Comment",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_MainSlideShow",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Post",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Schedule",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Ind.MainSlideShow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Page = table.Column<int>(nullable: false),
                    DatePublish = table.Column<DateTime>(nullable: false),
                    DateExpire = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    PostUrl = table.Column<string>(nullable: true),
                    imgUrl = table.Column<string>(nullable: true),
                    ShowState = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ind.MainSlideShow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ind.Post",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateEdited = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ShortContent = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    HaveComment = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    HeaderPic = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    SumRating = table.Column<long>(nullable: false),
                    RatingCount = table.Column<int>(nullable: false),
                    IsHighLight = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ind.Post", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ind.Comment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PostId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    HaveComformed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ind.Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ind.Comment_Ind.Comment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ind.Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ind.Comment_Ind.Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Ind.Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ind.Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PostId = table.Column<int>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ind.Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ind.Schedule_Ind.Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Ind.Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 3, 12, 23, 15, 128, DateTimeKind.Local).AddTicks(120), new Guid("2daa4af2-64f2-4dbf-ab27-e463acff0ca7") });

            migrationBuilder.CreateIndex(
                name: "IX_Ind.Comment_ParentId",
                table: "Ind.Comment",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ind.Comment_PostId",
                table: "Ind.Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Ind.Schedule_PostId",
                table: "Ind.Schedule",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ind.Comment");

            migrationBuilder.DropTable(
                name: "Ind.MainSlideShow");

            migrationBuilder.DropTable(
                name: "Ind.Schedule");

            migrationBuilder.DropTable(
                name: "Ind.Post");

            migrationBuilder.DropColumn(
                name: "Add_Comment",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_MainSlideShow",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Post",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_Schedule",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Comment",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_MainSlideShow",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Post",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Schedule",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Comment",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_MainSlideShow",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Post",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Schedule",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Comment",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_MainSlideShow",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Post",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Schedule",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 7, 2, 21, 4, 22, 875, DateTimeKind.Local).AddTicks(6818), new Guid("1fbc21ad-9a97-4155-8558-317f401ad0af") });
        }
    }
}
