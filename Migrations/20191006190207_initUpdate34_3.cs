using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate34_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Logs_Users_UserId",
            //     table: "Logs");

            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_Logs",
            //     table: "Logs");

            // migrationBuilder.RenameTable(
            //     name: "Logs",
            //     newName: "Log");

            // migrationBuilder.RenameIndex(
            //     name: "IX_Logs_UserId",
            //     table: "Log",
            //     newName: "IX_Log_UserId");

            // migrationBuilder.AlterColumn<int>(
            //     name: "Id",
            //     table: "Log",
            //     nullable: false,
            //     oldClrType: typeof(Guid))
            //     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_Log",
            //     table: "Log",
            //     column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 6, 22, 32, 5, 283, DateTimeKind.Local).AddTicks(8868), new Guid("ec8fea38-a84e-4062-8562-d2980e402a31") });

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Log_Users_UserId",
            //     table: "Log",
            //     column: "UserId",
            //     principalTable: "Users",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Log_Users_UserId",
            //     table: "Log");

            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_Log",
            //     table: "Log");

            // migrationBuilder.RenameTable(
            //     name: "Log",
            //     newName: "Logs");

            // migrationBuilder.RenameIndex(
            //     name: "IX_Log_UserId",
            //     table: "Logs",
            //     newName: "IX_Logs_UserId");

            // migrationBuilder.AlterColumn<Guid>(
            //     name: "Id",
            //     table: "Logs",
            //     nullable: false,
            //     oldClrType: typeof(int))
            //     .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_Logs",
            //     table: "Logs",
            //     column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 6, 22, 17, 7, 66, DateTimeKind.Local).AddTicks(1035), new Guid("898f58b4-27a9-4354-a2a0-0341340c81c7") });

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Logs_Users_UserId",
            //     table: "Logs",
            //     column: "UserId",
            //     principalTable: "Users",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }
    }
}
