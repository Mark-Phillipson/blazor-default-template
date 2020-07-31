using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSPApplication.Data.Migrations
{
    public partial class Notices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    NoticeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    Priority = table.Column<int>(maxLength: 30, nullable: false),
                    DatePosted = table.Column<DateTime>(nullable: false),
                    Show = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.NoticeId);
                });

            migrationBuilder.UpdateData(
                table: "Expenses",
                keyColumn: "ExpenseId",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2020, 7, 29, 15, 1, 8, 283, DateTimeKind.Local).AddTicks(4130));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notices");

            migrationBuilder.UpdateData(
                table: "Expenses",
                keyColumn: "ExpenseId",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2020, 7, 28, 17, 49, 27, 587, DateTimeKind.Local).AddTicks(3304));
        }
    }
}
