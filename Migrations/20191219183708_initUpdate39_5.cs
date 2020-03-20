using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate39_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_NotificationSeens_sm.Class_ClassId",
            //     table: "NotificationSeens");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_NotificationSeens_sm.Grade_GradeId",
            //     table: "NotificationSeens");

            // migrationBuilder.DropIndex(
            //     name: "IX_NotificationSeens_ClassId",
            //     table: "NotificationSeens");

            // migrationBuilder.DropIndex(
            //     name: "IX_NotificationSeens_GradeId",
            //     table: "NotificationSeens");

            // migrationBuilder.AlterColumn<int>(
            //     name: "GradeId",
            //     table: "NotificationSeens",
            //     nullable: false,
            //     oldClrType: typeof(int),
            //     oldNullable: true);

            // migrationBuilder.AlterColumn<int>(
            //     name: "ClassId",
            //     table: "NotificationSeens",
            //     nullable: false,
            //     oldClrType: typeof(int),
            //     oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderPicUrl",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "Attributes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "Attributes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 12, 19, 22, 7, 7, 304, DateTimeKind.Local).AddTicks(9228), new Guid("b50b6cb9-239e-41ea-a473-a0b4a104fd20") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderPicUrl",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "Attributes");

            // migrationBuilder.AlterColumn<int>(
            //     name: "GradeId",
            //     table: "NotificationSeens",
            //     nullable: true,
            //     oldClrType: typeof(int));

            // migrationBuilder.AlterColumn<int>(
            //     name: "ClassId",
            //     table: "NotificationSeens",
            //     nullable: true,
            //     oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 12, 13, 23, 33, 21, 607, DateTimeKind.Local).AddTicks(4396), new Guid("2256e8b2-17fc-4783-8ec8-b92bd15509cf") });

            // migrationBuilder.CreateIndex(
            //     name: "IX_NotificationSeens_ClassId",
            //     table: "NotificationSeens",
            //     column: "ClassId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_NotificationSeens_GradeId",
            //     table: "NotificationSeens",
            //     column: "GradeId");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_NotificationSeens_sm.Class_ClassId",
            //     table: "NotificationSeens",
            //     column: "ClassId",
            //     principalTable: "sm.Class",
            //     principalColumn: "Cls.Autonumber",
            //     onDelete: ReferentialAction.Restrict);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_NotificationSeens_sm.Grade_GradeId",
            //     table: "NotificationSeens",
            //     column: "GradeId",
            //     principalTable: "sm.Grade",
            //     principalColumn: "Grd.Autonumber",
            //     onDelete: ReferentialAction.Restrict);
        }
    }
}
