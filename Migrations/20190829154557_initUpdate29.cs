using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCMR_Api.Migrations
{
    public partial class initUpdate29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sm.Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    SenderType = table.Column<int>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    ReciverType = table.Column<int>(nullable: false),
                    ReciverId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.Ticket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sm.TicketConversation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Rate = table.Column<int>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    IsSender = table.Column<bool>(nullable: false),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sm.TicketConversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sm.TicketConversation_sm.Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "sm.Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 29, 20, 15, 56, 624, DateTimeKind.Local).AddTicks(9971), new Guid("d461cd09-bdd2-4288-aecb-fa44706b8980") });

            migrationBuilder.CreateIndex(
                name: "IX_sm.TicketConversation_TicketId",
                table: "sm.TicketConversation",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sm.TicketConversation");

            migrationBuilder.DropTable(
                name: "sm.Ticket");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdd", "GId" },
                values: new object[] { new DateTime(2019, 8, 27, 11, 38, 26, 307, DateTimeKind.Local).AddTicks(6718), new Guid("e0e60b55-e0ef-426b-bd65-2903f7443b09") });
        }
    }
}
