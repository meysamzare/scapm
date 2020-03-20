using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_sm.Student_Tbl_WorkBook_Header_Std.Code",
            //     table: "sm.Student");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_Tbl_WorkBook_Header_sm.Student_StdId",
            //     table: "Tbl_WorkBook_Header");

            // migrationBuilder.DropIndex(
            //     name: "IX_sm.Student_Std.Code",
            //     table: "sm.Student");

            // migrationBuilder.AlterColumn<int>(
            //     name: "StdId",
            //     table: "Tbl_WorkBook_Header",
            //     nullable: false,
            //     oldClrType: typeof(int))
            //     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "Add_Contract",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_ContractType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_PaymentType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Add_StdPayment",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_Contract",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_ContractType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_PaymentType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edit_StdPayment",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_Contract",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_ContractType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_PaymentType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Remove_StdPayment",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_Contract",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_ContractType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_PaymentType",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "View_StdPayment",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Fin.ContractType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Table = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fin.ContractType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fin.PaymentType",
                columns: table => new
                {
                    PatmentTypeAutonum = table.Column<int>(name: "PatmentType.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatmentTypeCode = table.Column<int>(name: "PatmentType.Code", nullable: false),
                    PatmentTypeTitle = table.Column<string>(name: "PatmentType.Title", nullable: true),
                    PatmentTypeDesc = table.Column<string>(name: "PatmentType.Desc", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fin.PaymentType", x => x.PatmentTypeAutonum);
                });

            migrationBuilder.CreateTable(
                name: "Fin.Contract",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    PartyContractId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    ContractTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fin.Contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fin.Contract_Fin.ContractType_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "Fin.ContractType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fin.StdPayment",
                columns: table => new
                {
                    StdPaymentAutonum = table.Column<int>(name: "StdPayment.Autonum", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StdPaymentRef = table.Column<string>(name: "StdPayment.Ref", nullable: true),
                    StdPaymentBank = table.Column<string>(name: "StdPayment.Bank", nullable: true),
                    StdPaymentAccNum = table.Column<string>(name: "StdPayment.AccNum", nullable: true),
                    StdPaymentBankSection = table.Column<string>(name: "StdPayment.BankSection", nullable: true),
                    StdPaymentAmount = table.Column<decimal>(name: "StdPayment.Amount", nullable: false),
                    StdPaymentPaymentTyp = table.Column<int>(name: "StdPayment.PaymentTyp", nullable: false),
                    StdPaymentStudent = table.Column<int>(name: "StdPayment.Student", nullable: false),
                    ContractId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fin.StdPayment", x => x.StdPaymentAutonum);
                    table.ForeignKey(
                        name: "FK_Fin.StdPayment_Fin.Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Fin.Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fin.StdPayment_Fin.PaymentType_StdPayment.PaymentTyp",
                        column: x => x.StdPaymentPaymentTyp,
                        principalTable: "Fin.PaymentType",
                        principalColumn: "PatmentType.Autonum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fin.StdPayment_sm.Student_StdPayment.Student",
                        column: x => x.StdPaymentStudent,
                        principalTable: "sm.Student",
                        principalColumn: "Std.Autonum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 30, 20, 6, 57, 370, DateTimeKind.Local).AddTicks(2996), new Guid("7d4fb1e3-a6ed-4c00-87a1-8e557a9e5c7c") });

            migrationBuilder.CreateIndex(
                name: "IX_Fin.Contract_ContractTypeId",
                table: "Fin.Contract",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fin.StdPayment_ContractId",
                table: "Fin.StdPayment",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Fin.StdPayment_StdPayment.PaymentTyp",
                table: "Fin.StdPayment",
                column: "StdPayment.PaymentTyp");

            migrationBuilder.CreateIndex(
                name: "IX_Fin.StdPayment_StdPayment.Student",
                table: "Fin.StdPayment",
                column: "StdPayment.Student");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fin.StdPayment");

            migrationBuilder.DropTable(
                name: "Fin.Contract");

            migrationBuilder.DropTable(
                name: "Fin.PaymentType");

            migrationBuilder.DropTable(
                name: "Fin.ContractType");

            migrationBuilder.DropColumn(
                name: "Add_Contract",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_ContractType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_PaymentType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Add_StdPayment",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_Contract",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_ContractType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_PaymentType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Edit_StdPayment",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_Contract",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_ContractType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_PaymentType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Remove_StdPayment",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_Contract",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_ContractType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_PaymentType",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "View_StdPayment",
                table: "Roles");

            migrationBuilder.AlterColumn<int>(
                name: "StdId",
                table: "Tbl_WorkBook_Header",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 6, 29, 19, 53, 41, 22, DateTimeKind.Local).AddTicks(6175), new Guid("1d9bafde-4a53-4155-b8bc-5a8e4a7db393") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.Student_Std.Code",
                table: "sm.Student",
                column: "Std.Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sm.Student_Tbl_WorkBook_Header_Std.Code",
                table: "sm.Student",
                column: "Std.Code",
                principalTable: "Tbl_WorkBook_Header",
                principalColumn: "StdId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_WorkBook_Header_sm.Student_StdId",
                table: "Tbl_WorkBook_Header",
                column: "StdId",
                principalTable: "sm.Student",
                principalColumn: "Std.Autonum",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
