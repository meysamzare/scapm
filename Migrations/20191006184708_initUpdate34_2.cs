using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate34_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AlterColumn<Guid>(
            //     name: "Id",
            //     table: "Logs",
            //     nullable: false,
            //     oldClrType: typeof(int))
            //     .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 6, 22, 17, 7, 66, DateTimeKind.Local).AddTicks(1035), new Guid("898f58b4-27a9-4354-a2a0-0341340c81c7") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AlterColumn<int>(
            //     name: "Id",
            //     table: "Logs",
            //     nullable: false,
            //     oldClrType: typeof(Guid))
            //     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 10, 2, 11, 31, 19, 54, DateTimeKind.Local).AddTicks(110), new Guid("dafe2757-8729-4d09-b1f2-a770c241d20c") });
        }
    }
}
