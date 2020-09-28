using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add_OnlineClass",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_OnlineClass",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_OnlineClass",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_OnlineClass",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "OnlineClasses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    meetingId = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    GradeId = table.Column<int>(nullable: false),
                    ClassId = table.Column<int>(nullable: true),
                    CourseId = table.Column<int>(nullable: false),
                    attendeePW = table.Column<string>(nullable: true),
                    moderatorPW = table.Column<string>(nullable: true),
                    welcome = table.Column<string>(nullable: true),
                    maxParticipants = table.Column<string>(nullable: true),
                    duration = table.Column<int>(nullable: false),
                    logoutURL = table.Column<string>(nullable: true),
                    meta = table.Column<string>(nullable: true),
                    copyright = table.Column<string>(nullable: true),
                    parentMeetingID = table.Column<string>(nullable: true),
                    sequence = table.Column<int>(nullable: false),
                    record = table.Column<bool>(nullable: false),
                    isBreakout = table.Column<bool>(nullable: false),
                    freeJoin = table.Column<bool>(nullable: false),
                    autoStartRecording = table.Column<bool>(nullable: false),
                    allowStartStopRecording = table.Column<bool>(nullable: false),
                    webcamsOnlyForModerator = table.Column<bool>(nullable: false),
                    muteOnStart = table.Column<bool>(nullable: false),
                    allowModsToUnmuteUsers = table.Column<bool>(nullable: false),
                    lockSettingsDisableCam = table.Column<bool>(nullable: false),
                    lockSettingsDisableMic = table.Column<bool>(nullable: false),
                    lockSettingsDisablePrivateChat = table.Column<bool>(nullable: false),
                    lockSettingsDisablePublicChat = table.Column<bool>(nullable: false),
                    lockSettingsDisableNote = table.Column<bool>(nullable: false),
                    lockSettingsLockedLayout = table.Column<bool>(nullable: false),
                    lockSettingsLockOnJoin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineClasses_sm.Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "sm.Class",
                        principalColumn: "Cls.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnlineClasses_sm.Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "sm.Course",
                        principalColumn: "Crs.Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnlineClasses_sm.Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "sm.Grade",
                        principalColumn: "Grd.Autonumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 9, 2, 17, 5, 34, 432, DateTimeKind.Local).AddTicks(306), new Guid("a38d0bf8-1616-41db-8d33-c7adae61df6f") });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClasses_ClassId",
                table: "OnlineClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClasses_CourseId",
                table: "OnlineClasses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClasses_GradeId",
                table: "OnlineClasses",
                column: "GradeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "Add_OnlineClass",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_OnlineClass",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_OnlineClass",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_OnlineClass",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2020, 8, 26, 7, 29, 29, 68, DateTimeKind.Local).AddTicks(8953), new Guid("a25f8872-b597-49a0-9485-2c6585ca84f1") });
        }
    }
}
