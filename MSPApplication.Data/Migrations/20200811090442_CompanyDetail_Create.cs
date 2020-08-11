using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSPApplication.Data.Migrations
{
    public partial class CompanyDetail_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "CompanyDetails",
    columns: table => new
    {
        Id = table.Column<int>(nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        Active = table.Column<bool>(nullable: false),
        CompanyName = table.Column<string>(maxLength: 100, nullable: false),
        AddressLine1 = table.Column<string>(maxLength: 255, nullable: true),
        AddressLine2 = table.Column<string>(maxLength: 255, nullable: true),
        City = table.Column<string>(maxLength: 30, nullable: true),
        StateProvinceCounty = table.Column<string>(maxLength: 40, nullable: true),
        Postcode = table.Column<string>(maxLength: 50, nullable: true),
        CountryId = table.Column<int>(nullable: false),
        WebAddress = table.Column<string>(maxLength: 100, nullable: true),
        PhoneNumber = table.Column<string>(maxLength: 20, nullable: true),
        EmailAddress = table.Column<string>(nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_CompanyDetails", x => x.Id);
        table.ForeignKey(
            name: "FK_CompanyDetails_Countries_CountryId",
            column: x => x.CountryId,
            principalTable: "Countries",
            principalColumn: "CountryId",
            onDelete: ReferentialAction.Cascade);
    });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CompanyDetails");

            migrationBuilder.DropTable(
                name: "Views_Assigned_Roles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "Expenses",
                keyColumn: "ExpenseId",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2020, 7, 29, 15, 1, 8, 283, DateTimeKind.Local).AddTicks(4130));
        }
    }
}
