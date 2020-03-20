using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
	public partial class initUpdate19 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// migrationBuilder.DropPrimaryKey(
			// 	name: "PK_Tbl_WorkBook_Header",
			// 	table: "Tbl_WorkBook_Header");

			// migrationBuilder.AlterColumn<int>(
			// 	name: "Id",
			// 	table: "Tbl_WorkBook_Header",
			// 	nullable: false,
			// 	oldClrType: typeof(int))
			// 	.OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

			migrationBuilder.AddColumn<int>(
				name: "Std.State",
				table: "sm.Student",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				name: "Std.BehaveState",
				table: "sm.StdClassMng",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				name: "Std.PayrollState",
				table: "sm.StdClassMng",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				name: "Std.StudyState",
				table: "sm.StdClassMng",
				nullable: false,
				defaultValue: 0);

			// migrationBuilder.AddPrimaryKey(
			// 	name: "PK_Tbl_WorkBook_Header",
			// 	table: "Tbl_WorkBook_Header",
			// 	column: "StdId");

			migrationBuilder.UpdateData(
				table: "Users",
				keyColumn: "Id",
				keyValue: 1,
				columns: new[] { "DateAdd", "GId" },
				values: new object[] { new DateTime(2019, 6, 29, 19, 53, 41, 22, DateTimeKind.Local).AddTicks(6175), new Guid("1d9bafde-4a53-4155-b8bc-5a8e4a7db393") });

			// migrationBuilder.CreateIndex(
			// 	name: "IX_sm.Student_Std.Code",
			// 	table: "sm.Student",
			// 	column: "Std.Code",
			// 	unique: true);

			// migrationBuilder.AddForeignKey(
			// 	name: "FK_sm.Student_Tbl_WorkBook_Header_Std.Code",
			// 	table: "sm.Student",
			// 	column: "Std.Code",
			// 	principalTable: "Tbl_WorkBook_Header",
			// 	principalColumn: "StdId",
			// 	onDelete: ReferentialAction.Cascade);

			// migrationBuilder.AddForeignKey(
			// 	name: "FK_Tbl_WorkBook_Header_sm.Student_StdId",
			// 	table: "Tbl_WorkBook_Header",
			// 	column: "StdId",
			// 	principalTable: "sm.Student",
			// 	principalColumn: "Std.Autonum",
			// 	onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// migrationBuilder.DropForeignKey(
			// 	name: "FK_sm.Student_Tbl_WorkBook_Header_Std.Code",
			// 	table: "sm.Student");

			// migrationBuilder.DropForeignKey(
			// 	name: "FK_Tbl_WorkBook_Header_sm.Student_StdId",
			// 	table: "Tbl_WorkBook_Header");

			// migrationBuilder.DropPrimaryKey(
			// 	name: "PK_Tbl_WorkBook_Header",
			// 	table: "Tbl_WorkBook_Header");

			// migrationBuilder.DropIndex(
			// 	name: "IX_sm.Student_Std.Code",
			// 	table: "sm.Student");

			migrationBuilder.DropColumn(
				name: "Std.State",
				table: "sm.Student");

			migrationBuilder.DropColumn(
				name: "Std.BehaveState",
				table: "sm.StdClassMng");

			migrationBuilder.DropColumn(
				name: "Std.PayrollState",
				table: "sm.StdClassMng");

			migrationBuilder.DropColumn(
				name: "Std.StudyState",
				table: "sm.StdClassMng");

			// migrationBuilder.AlterColumn<int>(
			// 	name: "Id",
			// 	table: "Tbl_WorkBook_Header",
			// 	nullable: false,
			// 	oldClrType: typeof(int))
			// 	.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

			// migrationBuilder.AddPrimaryKey(
			// 	name: "PK_Tbl_WorkBook_Header",
			// 	table: "Tbl_WorkBook_Header",
			// 	column: "Id");

			migrationBuilder.UpdateData(
				table: "Users",
				keyColumn: "Id",
				keyValue: 1,
				columns: new[] { "DateAdd", "GId" },
				values: new object[] { new DateTime(2019, 6, 22, 19, 56, 45, 17, DateTimeKind.Local).AddTicks(14), new Guid("58c95686-ad8f-4222-b9a0-1a6022469d87") });
		}
	}
}
